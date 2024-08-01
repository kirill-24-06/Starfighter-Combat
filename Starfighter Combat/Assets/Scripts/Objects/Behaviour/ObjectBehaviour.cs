using UnityEngine;

//Base behaviour;
public abstract class ObjectBehaviour : MonoBehaviour
{
    [SerializeField] protected float _objectSpeed;
    protected Vector2 _bordersOfExist = new Vector2(25.0f, 17.0f);
    protected IMover _objectMover;

    protected abstract void Initialise();

    protected void DeactivateOutOfBounds()
    {
        if (transform.position.y < -_bordersOfExist.y)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }

        if (transform.position.y > _bordersOfExist.y)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }

        if (transform.position.y < -_bordersOfExist.x)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }

        if (transform.position.y > _bordersOfExist.x)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}
