using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : Missile
{
    private ObjectHolder _enemies;

    protected override void Awake()
    {
        base.Awake();

        _enemies = EntryPoint.Instance.SpawnedObjects;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (EntryPoint.Instance.SpawnedObjects.FindRegisteredObject(collision.gameObject, ObjectTag.Enemy))
        {
            _events.EnemyDamaged?.Invoke(collision.gameObject, _data.Damage);
            Disable();
        }
    }

    protected override void OnHomingStart()
    {
        SeekNearestEnemy();
        base.OnHomingStart();
    }

    private void SeekNearestEnemy()
    {
        Transform nearestEnemy = null;

        List<GameObject> targets;
        float nearestEnemyDistance = Mathf.Infinity;

        targets = _enemies.GetRegisteredObjectsByTag(ObjectTag.Enemy);

        foreach (GameObject target in targets)
        {
            if (target.activeInHierarchy)
            {
                float currdistance = Vector2.Distance(transform.position, target.transform.position);

                if (currdistance < nearestEnemyDistance)
                {
                    nearestEnemy = target.transform;

                    nearestEnemyDistance = currdistance;
                }
            }
        }

        _target = nearestEnemy;
    }

}
