using UnityEngine;

public class EnemyProjectile : Projectile
{
    [SerializeField] private GameObject _collideEffect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Player.IsPlayer(collision.gameObject) || collision.gameObject == EntryPoint.Instance.Player.ForceField.gameObject)
        {
            Instantiate(_collideEffect, transform.position, _collideEffect.transform.rotation);
            Interact();
        }
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
