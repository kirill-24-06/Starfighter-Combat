using UnityEngine;

//Base behaviour;
public abstract class ObjectBehaviour : MonoBehaviour
{
    [SerializeField] protected ObjectsData _objectInfo;
    protected IMover _objectMoveHandler;

    public ObjectsData ObjectInfo => _objectInfo; 
}
