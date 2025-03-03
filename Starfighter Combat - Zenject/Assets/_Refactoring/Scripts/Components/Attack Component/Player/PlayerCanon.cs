using UnityEngine;
using Utils.SpawnSystem;

namespace Refactoring
{
    public class PlayerCanon : IWeapon, IResetable
    {
        private IWeaponData _data;

        private IFactory<MonoProduct> _projectileFactory;

        private Transform _firePoint;
        protected AudioSource _playerAudio;

        protected readonly Timer _reloadTimer;

        protected bool _isShooted = false;

        public PlayerCanon(Transform player,
            Timer timer,
            [Zenject.Inject(Id = "PlayerProjectile")] IFactory<MonoProduct> projectileFactory,
            IWeaponData data)
        {
            _data = data;

            _projectileFactory = projectileFactory;

            _firePoint = player.Find("ProjectilePosition");

            _playerAudio = player.GetComponentInChildren<AudioSource>();

            _reloadTimer = timer;
            _reloadTimer.TimeIsOver += OnReloadTimerExpired;
        }

        public virtual void Attack()
        {
            if (_isShooted) return;
            _isShooted = true;

            var projectile = _projectileFactory.Create();
            projectile.transform.SetLocalPositionAndRotation(_firePoint.position, _firePoint.rotation);

            _playerAudio.PlayOneShot(_data.FireSound, _data.FireSoundVolume);

            Reload();
        }

        private void Reload()
        {
            _reloadTimer.SetTimer(_data.ReloadCountDown);
            _reloadTimer.StartTimer();
        }

        public void Reset()
        {
            _reloadTimer.StopTimer();
            _isShooted = false;
        }

        protected void OnReloadTimerExpired() => _isShooted = false;
    }

}
