using UnityEngine;

public class PlayerProjectile : Projectile
{
    [SerializeField] private GameObject _collideEffect;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (EntryPoint.Instance.SpawnedObjects.FindRegisteredObject(collision.gameObject, ObjectTag.Enemy))
        {
            _events.EnemyDamaged?.Invoke(collision.gameObject, _data.Damage);

            Instantiate(_collideEffect, transform.position, _collideEffect.transform.rotation);
            Disable();
        }
    }
}
