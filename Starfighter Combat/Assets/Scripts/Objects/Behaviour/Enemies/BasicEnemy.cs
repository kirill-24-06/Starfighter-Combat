using UnityEngine;

//Performs all the logic of BasicBehavior, but can also take damage
public class BasicEnemy : BasicBehaviour
{
    protected IDamageble _healthHandler;

    private void Awake()
    {
        _objectMoveHandler = new ObjectBasicMove(this);
        _healthHandler = new Damageble(ObjectInfo.Health);
    }

    private void OnDisable()
    {
        _healthHandler.ResetHealth();
    }

    private void Update()
    {
        _objectMoveHandler.Move(Vector2.up, ObjectInfo.Speed);
        DeactivateOutOfBounds();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided");
        _healthHandler.TakeDamage(1);
        ObjectPoolManager.ReturnObjectToPool(collision.gameObject);
    }
}