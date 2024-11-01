using UnityEngine;

public class PlayerAttacker : IAttacker,IResetable
{
    protected readonly Player _player;
    private Transform _firePoint;
    protected GameObject _projectile;

    protected readonly Timer _reloadTimer;
    protected float _reloadTime;

    protected bool _isShooted = false;

    public PlayerAttacker(Player player)
    {
        _player = player;
        _firePoint = _player.transform.Find("ProjectilePosition");
        _projectile = _player.PlayerData.Projectile;

        _reloadTimer = new Timer(_player);
        _reloadTime = _player.PlayerData.ReloadTime;
        _reloadTimer.TimeIsOver += OnReloadTimerExpired;

    }

    public virtual void Fire()
    {
        if (!_isShooted)
        {
            _isShooted = true;

            ObjectPool.Get(_projectile, _firePoint.position, _firePoint.rotation);

            Reload();
        }
    }

    private void Reload()
    {
        if (!_player.gameObject.activeInHierarchy) return;

        _reloadTimer.SetTimer(_player.PlayerData.ReloadTime);
        _reloadTimer.StartTimer();
    }

    public void Reset()
    {
        _reloadTimer.StopTimer();
        _isShooted = false;
    }

    protected void OnReloadTimerExpired() => _isShooted = false;
}
