using UnityEngine;

public class PlayerMover : IMover
{
    private PlayerBehaviour _player;
    private Vector3 _direction;

    public PlayerMover(PlayerBehaviour player)
    {
        _player = player;
    }

    public void Move(Vector3 direction, float speed)
    {
        _direction = direction.normalized;
        _player.transform.Translate(speed * Time.deltaTime * _direction);
    }
}
