using UnityEngine;

public class EnemyProjectile : Projectile
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Player.IsPlayer(collision.gameObject) || collision.gameObject == EntryPoint.Instance.Player.ForceField.gameObject)
            Interact();
    }

    protected void Interact()
    {
        if (!EntryPoint.Instance.Player.IsInvunerable && !EntryPoint.Instance.Player.IsDroneActive)
            _events.PlayerDamaged?.Invoke(_data.Damage);

        else if (!EntryPoint.Instance.Player.IsInvunerable && EntryPoint.Instance.Player.IsDroneActive)
            _events.DroneDestroyed?.Invoke();

        Disable();
    }
}
