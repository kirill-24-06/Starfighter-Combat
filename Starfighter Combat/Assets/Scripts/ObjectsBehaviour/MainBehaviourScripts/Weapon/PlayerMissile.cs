using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : Missile
{
    [SerializeField] private GameObject _explosionPrefab;
    private Transform _transform;
    private MissileTargets _missileTargets;
    private Dictionary<Collider2D, IInteractableEnemy> _enemies;

    protected override void Awake()
    {
        base.Awake();

        _transform = transform;
        _missileTargets = EntryPoint.Instance.MissileTargets;
        _enemies = EntryPoint.Instance.CollisionMap.Interactables;
    }

    protected override void Start()
    {
        base.Start();
        EntryPoint.Instance.CollisionMap.RegisterNukeInteractable(GetComponent<Collider2D>(), this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_enemies.TryGetValue(collision.collider, out var enemy))
        {
            enemy.Interact();

            ObjectPoolManager.SpawnObject(_explosionPrefab, transform.position,
                _explosionPrefab.transform.rotation, ObjectPoolManager.PoolType.ParticleSystem);

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

        var nearestEnemyDistance = Mathf.Infinity;

        var targets = _missileTargets.PlayerMissileTargets;

        for (int i = 0; i < targets.Count; i++)
        {
            if (!targets[i].gameObject.activeSelf) continue;

            var currentDistance = Vector2.Distance(_transform.position, targets[i].position);

            if (currentDistance < nearestEnemyDistance)
            {
                nearestEnemy = targets[i];
                nearestEnemyDistance = currentDistance;
            }
        }
        _target = nearestEnemy;
    }

}
