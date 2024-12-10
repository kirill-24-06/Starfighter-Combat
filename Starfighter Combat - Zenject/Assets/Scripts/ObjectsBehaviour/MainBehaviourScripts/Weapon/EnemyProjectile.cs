using UnityEngine;

public class EnemyProjectile : Projectile
{
    [SerializeField] private GameObject _collideEffect;

    private void Start()
    {
        EntryPoint.Instance.CollisionMap.RegisterNukeInteractable(GetComponent<Collider2D>(), this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(_isPooled) return;
        _isPooled = true;

        if (Player.IsPlayer(collision.gameObject) || collision.gameObject == EntryPoint.Instance.Player.ForceField.gameObject)
        {
            ObjectPool.Get(_collideEffect, _transform.position,_collideEffect.transform.rotation);
            Collide();
        }
    }

    protected void Collide()
    {
        if (!EntryPoint.Instance.Player.IsInvunerable && !EntryPoint.Instance.Player.IsDroneActive)
            _events.PlayerDamaged?.Invoke(_data.Damage);

        else if (!EntryPoint.Instance.Player.IsInvunerable && EntryPoint.Instance.Player.IsDroneActive)
            _events.DroneDestroyed?.Invoke();

        ObjectPool.Release(_gameObject);
    }
}
