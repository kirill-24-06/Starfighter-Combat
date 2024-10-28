using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    [SerializeField] private GameObject _collideEffect;
    private Dictionary<Collider2D,IInteractableEnemy> _enemies;

    protected override void Awake()
    {
        base.Awake();
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

            ObjectPoolManager.SpawnObject(_collideEffect, transform.position,
                _collideEffect.transform.rotation, ObjectPoolManager.PoolType.ParticleSystem);

            Disable();
        }
    }
}