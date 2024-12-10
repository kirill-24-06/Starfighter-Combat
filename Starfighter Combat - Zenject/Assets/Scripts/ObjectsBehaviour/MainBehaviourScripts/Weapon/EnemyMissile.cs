using UnityEngine;

public class EnemyMissile : Missile
{
    private void Start()
    {
        EntryPoint.Instance.CollisionMap.RegisterNukeInteractable(GetComponent<Collider2D>(), this);
    }

    protected override void OnHomingStart()
    {
        LockOnTarget();
        base.OnHomingStart();
    }

    private void LockOnTarget() => _target = EntryPoint.Instance.Player.transform;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isPooled) return;
        _isPooled = true;

        if (Player.IsPlayer(collision.gameObject) || collision.gameObject == EntryPoint.Instance.Player.ForceField.gameObject)
        {
            ObjectPool.Get(_explosionPrefab, _transform.position,
                _explosionPrefab.transform.rotation);

            Interact();
        }
    }

    protected void Interact()
    {
        if (!EntryPoint.Instance.Player.IsInvunerable && !EntryPoint.Instance.Player.IsDroneActive)
            _events.PlayerDamaged?.Invoke(1);

        else if (!EntryPoint.Instance.Player.IsInvunerable && EntryPoint.Instance.Player.IsDroneActive)
            _events.DroneDestroyed?.Invoke();

        ObjectPool.Release(_gameObject);
    }
}