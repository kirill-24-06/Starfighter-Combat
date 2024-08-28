using UnityEngine;

public class Mover : IMover
{
    private readonly Transform _client;

    public Mover(Transform client)
    {
        _client = client;
    }

    public void Move(Vector3 direñtion, float speed)
    {
        _client.transform.Translate(speed * Time.deltaTime * direñtion);
    }
}
