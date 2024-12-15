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
        _client.Translate(speed * Time.deltaTime * dire�tion);
    }
}

public interface IMovementData
{
    Vector2 GameBorders { get; set; }
}
