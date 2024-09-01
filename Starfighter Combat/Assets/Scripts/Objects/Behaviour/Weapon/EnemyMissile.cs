using UnityEngine;

public class EnemyMissile : Missile
{
    protected override void OnHomingStart()
    {
        LockOnTarget();
        base.OnHomingStart();
    }

    private void LockOnTarget() => _target = EntryPoint.Instance.Player.transform;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Player.IsPlayer(collision.gameObject) || collision.gameObject == EntryPoint.Instance.Player.ForceField.gameObject)
            Interact();
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