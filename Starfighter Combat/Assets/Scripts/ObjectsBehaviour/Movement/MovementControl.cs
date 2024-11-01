using System;
using UnityEngine;

public class MovementControl: IResetable
{
    private readonly Transform _client;
    private readonly Vector2 _area;
    private readonly float _minY;

    protected IMover _mover;
    protected Mover _forwardMover;
    protected AdvancedMover _randomDirectionMover;

    protected Vector3 _direction = Vector2.up;
    protected Vector3 _randomDirection = Vector3.zero;

    private bool _isArrived = false;

    public Action Arrival;

    public bool IsMoving { get; private set; }

    public MovementControl(Transform client, IMovableData movableData)
    {
        _client = client;
        _area = movableData.Area;
        _minY = movableData.MinY;

        _forwardMover = new Mover(client);
        _randomDirectionMover = new AdvancedMover(client);
        _mover = _forwardMover;

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
        _mover = _forwardMover;
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

        _mover = _randomDirectionMover;
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
        _randomDirection.x = UnityEngine.Random.Range(-_area.x, _area.x);
        _randomDirection.y = UnityEngine.Random.Range(_minY, _area.y);

        return _randomDirection;
    }

    public void Reset()
    {
        _isArrived = false;

        _mover = _forwardMover;
        _direction = Vector3.up;
    }
}