using System;
using UnityEngine;

public class Bonus : BasicObject
{
    //Data
    [SerializeField] BonusData _data;

    protected Timer _liveTimer;
    private Timer _positionChangeTimer;

    //Movement
    private IMover _mover;
    private Vector3 _direction;

    private bool _isPositionChangeTimerStart = false;
    private bool _isArrived = false;

    private event Action Arrival;

    private void Awake()
    {
        _liveTimer = new Timer(this);
        _liveTimer.TimeIsOver += OnLiveTimerExpired;

        _positionChangeTimer = new Timer(this);
        _positionChangeTimer.TimeIsOver += OnPositionChangeTimerExpired;

        Arrival += OnArrival;

        PoolMap.SetParrentObject(gameObject, GlobalConstants.PoolTypesByTag[_data.Tag]);
    }

    private void OnEnable()
    {
        _isArrived = false;

        ChangeMover(new Mover(transform));
        _direction = Vector3.up;
    }

    private void Update()
    {
        Move();
        DeactivateOutOfBounds();

        if (!_isArrived)
        {
            CheckArrival();
        }

        if (transform.position == _direction)
        {
            if (!_isPositionChangeTimerStart)
            {
                _isPositionChangeTimerStart = true;

                _positionChangeTimer.SetTimer(_data.PositionChangeTime);
                _positionChangeTimer.StartTimer();
            }
        }
    }

    private void OnDisable()
    {
        _liveTimer.StopTimer();
        _positionChangeTimer.StopTimer();
        _isPositionChangeTimerStart = false;
        EntryPoint.Instance.Events.BonusTaken?.Invoke();
    }

    protected override void Move()
    {
       _mover.Move(_direction,_data.Speed);
    }

    protected override void Disable()
    {
        ObjectPool.Release(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_data.BonusTag != BonusTag.None)
        {
            if (Player.IsPlayer(collision.gameObject))
                Interact();
        }
    }

    private void Interact()
    {
        _events.BonusCollected?.Invoke(_data.BonusTag);
        _events.AddScore?.Invoke(_data.Score);

        Disable();
    }

    private void OnArrival()
    {
        _isArrived = true;

        ChangeMover(new AdvancedMover(transform));
        SetNewDirection();

        _liveTimer.SetTimer(_data.LiveTime);
        _liveTimer.StartTimer();
    }

    private void OnLiveTimerExpired()
    {
        _positionChangeTimer.StopTimer();
        ChangeMover(new Mover(transform));
        SetNewDirection();
    }

    private void OnPositionChangeTimerExpired()
    {
        SetNewDirection();
        _isPositionChangeTimerStart = false;
    }

   private void CheckArrival()
    {
        bool isArrived = (transform.position.x > -_data.GameArea.x 
            || transform.position.x < _data.GameArea.x) && transform.position.y < _data.GameArea.y;

        if (isArrived)
        {
            Arrival?.Invoke();
        }
    }

    private void ChangeMover(IMover newMover)
    {
       _mover = newMover;
    }

    private void SetNewDirection()
    {
        switch (_mover)
        {
            case Mover:

                _direction = Vector2.up;
                break;

            case AdvancedMover:

                _direction = GenerateMovePoint();
                break;
        }
    }

    private Vector3 GenerateMovePoint()
    {
        float positionX = UnityEngine.Random.Range(-_data.GameArea.x, _data.GameArea.x);
        float positionY = UnityEngine.Random.Range(-_data.GameArea.y, _data.GameArea.y);

        return new Vector3(positionX, positionY, 0);
    }

    private void DeactivateOutOfBounds()
    {
        if (transform.position.y < -_data.DisableBorders.y)
        {
            Disable();
        }

        if (transform.position.y > _data.DisableBorders.y)
        {
            Disable();
        }

        if (transform.position.x < -_data.DisableBorders.x)
        {
            Disable();
        }

        if (transform.position.x > _data.DisableBorders.x)
        {
            Disable();
        }
    } 
}