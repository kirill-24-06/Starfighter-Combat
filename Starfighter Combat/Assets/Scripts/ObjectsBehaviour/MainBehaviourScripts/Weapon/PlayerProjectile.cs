using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    [SerializeField] private GameObject _collideEffect;
    private Dictionary<Collider2D,IInteractableEnemy> _enemies;

    private void Start()
    {
        EntryPoint.Instance.CollisionMap.RegisterNukeInteractable(GetComponent<Collider2D>(), this);
        _enemies = EntryPoint.Instance.CollisionMap.Interactables;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_enemies.TryGetValue(collision.collider, out var enemy))
        {
            enemy.Interact();

            ObjectPool.Get(_collideEffect, transform.position,
                _collideEffect.transform.rotation);

            ObjectPool.Release(_gameObject);
        }
    }
}