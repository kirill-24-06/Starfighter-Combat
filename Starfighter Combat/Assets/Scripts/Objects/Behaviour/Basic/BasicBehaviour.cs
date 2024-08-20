using UnityEngine;

//An object with this behavior just moves forward
//It is used for laser projectiles
public class BasicBehaviour : ObjectBehaviour
{
    private void Awake()
    {
        _objectMoveHandler = new ObjectBasicMove(this);
        EventManager.GetInstance().IonSphereUse += OnIonSphereUse;
    }

    private void Update()
    {
        _objectMoveHandler.Move(Vector2.up, ObjectInfo.Speed);
        DeactivateOutOfBounds();
    }

    private void OnDestroy()
    {
        EventManager.GetInstance().IonSphereUse -= OnIonSphereUse;
    }

    private void OnIonSphereUse()
    {
        if (gameObject.activeInHierarchy)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }

    protected void DeactivateOutOfBounds()
    {
        if (transform.position.y < -ObjectInfo.GameZoneBorders.y)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }

        if (transform.position.y > ObjectInfo.GameZoneBorders.y)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }

        if (transform.position.x < -ObjectInfo.GameZoneBorders.x)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }

        if (transform.position.x > ObjectInfo.GameZoneBorders.x)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}
