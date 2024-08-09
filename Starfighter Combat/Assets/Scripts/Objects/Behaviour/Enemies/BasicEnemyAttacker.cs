using UnityEngine;


//Performs all the logic of BasicEnemy, but also can attack
public class BasicEnemyAttacker : BasicEnemy
{
    private IAttacker _enemyAttackHandler;


    private void Awake()
    {
        _objectMoveHandler = new ObjectBasicMove(this);
        _healthHandler = new Damageble(this);
        _enemyAttackHandler = new Attacker(this);
    }

    private void OnEnable()
    {
        _enemyAttackHandler.Reset();
    }

    private void OnDisable()
    {
        _healthHandler.ResetHealth();
    }

    private void Update()
    {
        _objectMoveHandler.Move(Vector2.up, ObjectInfo.Speed);
        DeactivateOutOfBounds();

        _enemyAttackHandler.Fire(ObjectInfo.Projectile);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerWeapon"))
        {
            Debug.Log("Collided");
            _healthHandler.TakeDamage(1);
            ObjectPoolManager.ReturnObjectToPool(collision.gameObject);
        }
    }
}