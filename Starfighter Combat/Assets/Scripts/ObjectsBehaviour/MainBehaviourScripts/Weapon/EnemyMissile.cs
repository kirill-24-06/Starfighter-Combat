using UnityEngine;

public class EnemyMissile : Missile
{
    [SerializeField] private GameObject _explosionPrefab;

    protected override void OnHomingStart()
    {
        LockOnTarget();
        base.OnHomingStart();
    }

    private void LockOnTarget() => _target = EntryPoint.Instance.Player.transform;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Player.IsPlayer(collision.gameObject) || collision.gameObject == EntryPoint.Instance.Player.ForceField.gameObject)
        {
            Instantiate(_explosionPrefab, transform.position, _explosionPrefab.transform.rotation);
            Interact();
        }

    }

    protected void Interact()
    {
        if (!EntryPoint.Instance.Player.IsInvunerable && !EntryPoint.Instance.Player.IsDroneActive)
        {
            _events.PlayerDamaged?.Invoke(1);
        }

        else if (!EntryPoint.Instance.Player.IsInvunerable && EntryPoint.Instance.Player.IsDroneActive)
        {
            _events.DroneDestroyed?.Invoke();
        }

        Disable();
    }
}