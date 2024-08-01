using UnityEngine;

//An object with this behavior just moves forward
//It is used for laser projectiles
public class BasicBehaviour : ObjectBehaviour
{
    private void Awake()
    {
        Initialise();
    }

    protected override void Initialise()
    {
        _objectMover = new ObjectBasicMove(this);
    }

    private void Update()
    {
        _objectMover.Move(Vector2.up, _objectSpeed);
        DeactivateOutOfBounds();
    }
}
