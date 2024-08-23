using UnityEngine;

//Base behaviour;
public abstract class ObjectBehaviour : MonoBehaviour
{
    protected IMover _objectMoveHandler;

    [SerializeField] protected ObjectsData _objectInfo;
    public ObjectsData ObjectInfo => _objectInfo;

}
