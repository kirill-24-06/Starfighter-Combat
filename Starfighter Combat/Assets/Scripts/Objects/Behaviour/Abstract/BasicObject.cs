using UnityEngine;

public abstract class BasicObject : MonoBehaviour
{
    protected EventManager _events;

    protected abstract void Disable();

    protected abstract void Move();

    protected virtual void Start()
    {
        _events = EntryPoint.Instance.Events;
    }
}
