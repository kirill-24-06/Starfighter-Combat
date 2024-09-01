using UnityEngine;

public class PlayerProjectile : Projectile
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (EntryPoint.Instance.SpawnedObjects.FindRegisteredObject(collision.gameObject, ObjectTag.Enemy))
        {
            _events.EnemyDamaged?.Invoke(collision.gameObject, _data.Damage);
            Disable();
        }
    }
}
