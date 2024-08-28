using UnityEngine;

public class Mover : IMover
{
    private readonly Transform _client;

    public Mover(Transform client)
    {
        _client = client;
    }

    public void Move(Vector3 dire�tion, float speed)
    {
        _client.transform.Translate(speed * Time.deltaTime * dire�tion);
    }
}
