using UnityEngine;

public class AdvancedMover : IMover
{
    private readonly Transform _client;

    public AdvancedMover(Transform objectBehaviour)
    {
        _client = objectBehaviour;
    }

    public void Move(Vector3 direñtion, float speed)
    {
        _client.position = Vector3.MoveTowards(_client.position, direñtion, speed * Time.deltaTime);
    }
}
