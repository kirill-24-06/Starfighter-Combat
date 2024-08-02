using System;
using UnityEngine;

//Unlike the basic behavior, it can change the type of movement
public class AdvancedBehaviour : ObjectBehaviour
{
    [Header("After the expiration, the object will leave the game area")]
    [SerializeField] private float _liveTime = 30.0f;
    private Timer _liveTimer;

    [Header("After the expiration, the object will change its position in the game area")]
    [SerializeField] private float _positionChangeTime = 7.0f;
    private Timer _positionChangeTimer;

    protected Vector3 _direction = Vector3.up;
    protected Vector2 _gameZoneBorders = new Vector2(19.0f, 10.0f);
    private bool _isArrived = false;

    protected event Action Arrival;

    private void Awake()
    {
        _liveTimer = new Timer(this);
        _liveTimer.TimeIsOver += OnLiveTimerExpired;

        _positionChangeTimer = new Timer(this);
        _positionChangeTimer.TimeIsOver += OnPositionChangeTimerExpired;

        Arrival += OnArrival;
    }

    private void OnEnable()
    {
        _isArrived = false;
        _objectMover = new ObjectBasicMove(this);
    }

    private void OnDisable()
    {
        _liveTimer.StopTimer();
        _positionChangeTimer.StopTimer();
    }

    private void Update()
    {
        _objectMover.Move(_direction, _objectSpeed);
        DeactivateOutOfBounds();

        if (!_isArrived)
        {
            CheckArrival();
        }

        if (transform.position == _direction)
        {
            _positionChangeTimer.SetTimer(_positionChangeTime);
            _positionChangeTimer.StartTimer();
        }
    }

    protected void OnArrival()
    {
        _isArrived = true;

        ChangeMover(new ObjectAdvancedMove(this));
        SetNewDirection();

        _liveTimer.SetTimer(_liveTime);
        _liveTimer.StartTimer();
    }

    protected void OnLiveTimerExpired()
    {
        _positionChangeTimer.StopTimer();
        ChangeMover(new ObjectBasicMove(this));
        SetNewDirection();
    }

    public void OnPositionChangeTimerExpired()
    {
        SetNewDirection();
    }

    protected void CheckArrival()
    {
        if (transform.position.x > -_gameZoneBorders.x && transform.position.y < _gameZoneBorders.y)
        {
            Arrival?.Invoke();
        }

        if (transform.position.x < _gameZoneBorders.x && transform.position.y < _gameZoneBorders.y)
        {
            Arrival?.Invoke();
        }
    }

    protected void ChangeMover(IMover newMover)
    {
        _objectMover = newMover;
    }

    protected void SetNewDirection()
    {
        switch (_objectMover)
        {
            case ObjectBasicMove:

                _direction = Vector2.up;
                break;
            case ObjectAdvancedMove:

                _direction = GenerateMovePoint();
                break;
        }
    }

    protected Vector3 GenerateMovePoint()
    {
        float positionX = UnityEngine.Random.Range(-_gameZoneBorders.x, _gameZoneBorders.x);
        float positionY = UnityEngine.Random.Range(-_gameZoneBorders.y, _gameZoneBorders.y);

        return new Vector3(positionX, positionY, 0);
    }
}
