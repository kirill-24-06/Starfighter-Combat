using UnityEngine;

public class PlayerProjectile : Projectile
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (EntryPoint.Instance.SpawnedObjects.FindRegisteredObject(collision.gameObject, ObjectTag.Enemy))
        {
            _events.EnemyDamaged?.Invoke(collision.gameObject, _data.Damage);
            Disable();
        }
    }
}
