using UnityEngine;

public class PlayerMover : IMover
{
    private PlayerController _playerController;
    private Vector2 _direction;

    public PlayerMover(PlayerController playerController)
    {
        _playerController = playerController;
    }

    public void Move(Vector2 direction, float speed)
    {
        _direction = direction.normalized;
        _playerController.transform.Translate(speed * Time.deltaTime * _direction);
    }
}
