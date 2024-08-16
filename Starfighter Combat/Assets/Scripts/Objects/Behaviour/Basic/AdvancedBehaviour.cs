using System;
using UnityEngine;

//Unlike the basic behavior, it can change the type of movement on Timer
public class AdvancedBehaviour : BasicBehaviour
{
    [Header("After the expiration, the object will leave the game area")]
    [SerializeField] protected float _liveTime = 30.0f;
    protected Timer _liveTimer;

    [Header("After the expiration, the object will change its position in the game area")]
    [SerializeField] private float _positionChangeTime = 7.0f;
    private Timer _positionChangeTimer;
    private bool _isPositionChangeTimerStart = false;

    protected Vector3 _direction;
    private Vector2 _gameZoneBorders = new Vector2(19.0f, 10.0f);
    protected bool _isArrived = false;

    protected event Action Arrival;

    private void Awake()
    {
        Initialise();
    }

    private void OnEnable()
    {
        _isArrived = false;

        ChangeMover(new ObjectBasicMove(this));
        _direction = Vector3.up;
    }

    private void Update()
    {
        _objectMoveHandler.Move(_direction, ObjectInfo.Speed);
        DeactivateOutOfBounds();

        if (!_isArrived)
        {
            CheckArrival();
        }

        if (transform.position == _direction)
        {
            if (!_isPositionChangeTimerStart)
            {
                _positionChangeTimer.SetTimer(_positionChangeTime);
                _positionChangeTimer.StartTimer();
                _isPositionChangeTimerStart = true;
            }
        }
    }

    private void OnDisable()
    {
        _liveTimer.StopTimer();
        StopAllCoroutines();
        _positionChangeTimer.StopTimer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(ObjectInfo.BonusTag != BonusTag.None)
        {
            if (PlayerBehaviour.IsPlayer(collision) && !EntryPoint.Player.IsTaken)
            {
                EventManager.GetInstance().BonusCollected?.Invoke(ObjectInfo.BonusTag);
                ObjectPoolManager.ReturnObjectToPool(gameObject);
            }

            else if (PlayerBehaviour.IsPlayer(collision) && ObjectInfo.BonusTag == BonusTag.Health)
            {
                EventManager.GetInstance().BonusCollected?.Invoke(ObjectInfo.BonusTag);
                ObjectPoolManager.ReturnObjectToPool(gameObject);
            }
        }

        else
        {
            Debug.LogWarning("the wrong behavior is being used" + this);
        }
    }

    protected void Initialise()
    {
        _liveTimer = new Timer(this);
        _liveTimer.TimeIsOver += OnLiveTimerExpired;

        _positionChangeTimer = new Timer(this);
        _positionChangeTimer.TimeIsOver += OnPositionChangeTimerExpired;

        Arrival += OnArrival;
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

    protected void OnPositionChangeTimerExpired()
    {
        SetNewDirection();
        _isPositionChangeTimerStart = false;
    }

    protected void CheckArrival()
    {
        bool isArrived = (transform.position.x > -_gameZoneBorders.x || transform.position.x < _gameZoneBorders.x) && transform.position.y < _gameZoneBorders.y;

        if (isArrived)
        {
            Arrival?.Invoke();
        }
    }

    protected void ChangeMover(IMover newMover)
    {
        _objectMoveHandler = newMover;
    }

    protected virtual void SetNewDirection()
    {
        switch (_objectMoveHandler)
        {
            case ObjectBasicMove:

                _direction = Vector2.up;
                break;

            case ObjectAdvancedMove:

                _direction = GenerateMovePoint();
                break;
        }
    }

    protected virtual Vector3 GenerateMovePoint()
    {
        float positionX = UnityEngine.Random.Range(-_gameZoneBorders.x, _gameZoneBorders.x);
        float positionY = UnityEngine.Random.Range(-_gameZoneBorders.y, _gameZoneBorders.y);

        return new Vector3(positionX, positionY, 0);
    }
}