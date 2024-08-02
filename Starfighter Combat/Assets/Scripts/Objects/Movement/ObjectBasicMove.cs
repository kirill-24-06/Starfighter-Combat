using UnityEngine;

public class ObjectBasicMove : IMover
{
    private readonly ObjectBehaviour _objectBehaviour;

    public ObjectBasicMove(ObjectBehaviour objectBechaviour)
    {
        _objectBehaviour = objectBechaviour;
    }
    public virtual void Move(Vector3 direñtion, float speed)
    {
        _objectBehaviour.transform.Translate(speed * Time.deltaTime * direñtion);
    }
}