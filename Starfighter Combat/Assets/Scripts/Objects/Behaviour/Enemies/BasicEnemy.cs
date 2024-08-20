using UnityEngine;

//Performs all the logic of BasicBehavior, but can also take damage
public class BasicEnemy : BasicBehaviour
{
    protected IDamageble _healthHandler;

    private void Awake()
    {
        _objectMoveHandler = new ObjectBasicMove(this);
        _healthHandler = new Damageble(this);
        EventManager.GetInstance().IonSphereUse += OnIonSphereUse;
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

    protected void OnIonSphereUse()
    {
        if (gameObject.activeInHierarchy)
        {
            _healthHandler.TakeDamage(10);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ObjectHolder.GetInstance().FindRegisteredObject(collision.gameObject, ObjectTag.PlayerWeapon))
        {
            _healthHandler.TakeDamage(1);
            ObjectPoolManager.ReturnObjectToPool(collision.gameObject);
        }
    }

    private void OnDestroy()
    {
        EventManager.GetInstance().IonSphereUse -= OnIonSphereUse;
    }
}