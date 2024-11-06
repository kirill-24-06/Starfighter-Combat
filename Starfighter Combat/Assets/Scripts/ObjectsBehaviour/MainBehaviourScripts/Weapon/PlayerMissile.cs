using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : Missile
{
    private Collider2D[] _missileTargets;
    private Queue<Transform> _targetsQueue;
    private CollisionMap _collisionMap;

    private void Start()
    {
        _transform = transform;
        _missileTargets = new Collider2D[35];
        _targetsQueue = new Queue<Transform>();
        _collisionMap = EntryPoint.Instance.CollisionMap;

        EntryPoint.Instance.CollisionMap.RegisterNukeInteractable(GetComponent<Collider2D>(), this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isPooled) return;
        _isPooled = true;

        if (_collisionMap.Interactables.TryGetValue(collision.collider, out var enemy))
            enemy.Interact();

        ObjectPool.Get(_explosionPrefab, _transform.position, _explosionPrefab.transform.rotation);

        ObjectPool.Release(_gameObject);
    }

    protected override void OnHomingStart()
    {
        LockOnTarget();
        SeekNearestEnemy();
        base.OnHomingStart();
    }

    private void SeekNearestEnemy()
    {
        Transform nearestEnemy = null;

        var nearestEnemyDistance = Mathf.Infinity;

        var count = _targetsQueue.Count;

        for (int i = 0; i < count; i++)
        {
            var target = _targetsQueue.Dequeue();
            var currentDistance = Vector2.Distance(_transform.position, target.position);

            if (currentDistance < nearestEnemyDistance)
            {
                nearestEnemy = target;
                nearestEnemyDistance = currentDistance;
            }
        }

        _target = nearestEnemy;
    }

    private void LockOnTarget()
    {
        var targets = Physics2D.OverlapCircleNonAlloc(_transform.position,25f, _missileTargets);

        for (int i = 0; i < targets; i++)
        {
            if (_collisionMap.PlayerMissileTargets.Contains(_missileTargets[i].transform))
            {
                _targetsQueue.Enqueue(_missileTargets[i].transform);
            }
        }
    }
}