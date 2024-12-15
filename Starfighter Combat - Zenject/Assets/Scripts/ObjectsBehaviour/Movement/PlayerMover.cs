using UnityEngine;

public class PlayerMover : IMover
{
    private readonly Transform _client;
    private Vector2 _borders;

    public PlayerMover(Transform client, IMovementData data)
    {
        _client = client;
        _borders = data.GameBorders;
    }

    public void Move(Vector3 direсtion, float speed)
    {
        speed *= Time.deltaTime;
        _client.Translate(speed * direсtion);

        CheckBorders();
    }

    private void CheckBorders()
    {
        if (_client.position.x < -_borders.x)
            _client.position = new Vector3(-_borders.x, _client.position.y, _client.position.z);
        if (_client.position.x > _borders.x)
            _client.position = new Vector3(_borders.x, _client.position.y, _client.position.z);
        if (_client.position.y < -_borders.y)
            _client.position = new Vector3(_client.position.x, -_borders.y, _client.position.z);

        if (_client.position.y > _borders.y)
            _client.position = new Vector3(_client.position.x, _borders.y, _client.position.z);
    }
}
