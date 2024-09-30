using System;
using UnityEngine;

public class AdvancedMove
{
    private readonly Transform _client;
    private readonly Vector2 _area;

    protected IMover _mover;
    protected Vector3 _direction = Vector2.up;

    private bool _isArrived = false;

    public Action Arrival;

    public bool IsMoving { get; private set; }

    public AdvancedMove(Transform client, Vector2 triggerArea)
    {
        _client = client;
        _area = triggerArea;

        _mover = new Mover(client);

        Arrival += OnArrive;
    }

    public void Move(float speed)
    {
        _mover.Move(_direction, speed);

        CheckArrival();
        IsMoving = _client.position != _direction;
    }

    public void Disengage()
    {
        _mover = new Mover(_client);
        SetNewDirection();
    }

    private void CheckArrival()
    {
        if (!_isArrived)
        {
            bool isArrived =
           (_client.position.x > -_area.x ||
           _client.position.x < _area.x) &&
           _client.position.y < _area.y;

            if (isArrived)
            {
                Arrival?.Invoke();
            }
        }
    }
    private void OnArrive()
    {
        _isArrived = true;

        _mover = new AdvancedMover(_client);
        SetNewDirection();
    }

    public virtual void SetNewDirection()
    {
        switch (_mover)
        {
            case Mover:

                _direction = Vector2.up;
                break;

            case AdvancedMover:

                _direction = GenerateMovePoint();
                LookInTargetDirection(_direction);   
                break;
        }
    }

    public void LookInTargetDirection(Vector3 target)
    {
        float rotation = Mathf.Atan2(target.y - _client.position.y, target.x - _client.position.x) * Mathf.Rad2Deg - 90;
        _client.rotation = Quaternion.Euler(_client.rotation.eulerAngles.x, _client.rotation.eulerAngles.y, rotation);
    }

    protected virtual Vector3 GenerateMovePoint()
    {
        float positionX = UnityEngine.Random.Range(-_area.x, _area.x);
        float positionY = UnityEngine.Random.Range(- _area.y/5, _area.y);

        return new Vector3(positionX, positionY, 0);
    }

    public void Reset()
    {
        _isArrived = false;

        _mover = new Mover(_client);
        _direction = Vector3.up;
    }
}
