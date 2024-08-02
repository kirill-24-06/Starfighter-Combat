using UnityEngine;

public class ObjectAdvancedMove : IMover
{
    private readonly ObjectBehaviour _objectBehaviour;

    public ObjectAdvancedMove(ObjectBehaviour objectBehaviour)
    {
        _objectBehaviour = objectBehaviour;
    }

    public void Move(Vector3 dire�tion, float speed)
    {
        _objectBehaviour.transform.position = Vector3.MoveTowards(_objectBehaviour.transform.position, dire�tion, speed * Time.deltaTime);
    }
}
