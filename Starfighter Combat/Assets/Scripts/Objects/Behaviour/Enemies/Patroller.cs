using UnityEngine;

public class Patroller : AdvancedEnemy
{
    private Bounds[] _patrolAreas = null;
    private Vector3[] _movePoints = null;
    private int _point = 0;

    private void Awake()
    {
        Initialise();

        _enemyHealthHandler = new Damageble(this);
        _enemyAttackHandler = new MultipleCanonsAttacker(this);

        EventManager.GetInstance().IonSphereUse += OnIonSphereUse;
        EventManager.GetInstance().Fire += OnFire;
        EventManager.GetInstance().PlayerDied += OnPlayerDied;
    }

    private void OnEnable()
    {
        _isArrived = false;

        ChangeMover(new ObjectBasicMove(this));
        _direction = Vector3.up;

        _enemyAttackHandler.Reset();
        _enemyHealthHandler.ResetHealth();
    }

    private void Start()
    {
        _player = EntryPoint.Instance.Player.transform;

        _patrolAreas = EntryPoint.Instance.PatrolArea;
        _movePoints = new Vector3[_patrolAreas.Length];
        NewMovePoints();
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
            LookInTargetDirection(_player.position);
            _enemyAttackHandler.Fire(ObjectInfo.Projectile);
        }

        if (_shotsFired >= _shotsBeforePositionChange)
        {
            SetNewDirection();
            _shotsFired = 0;
        }
    }

    private void OnDisable()
    {
        _liveTimer.StopTimer();
        StopAllCoroutines();
        _shotsFired = 0;
        NewMovePoints();
    }

    private void OnDestroy()
    {
        EventManager.GetInstance().IonSphereUse -= OnIonSphereUse;
        EventManager.GetInstance().Fire -= OnFire;
        EventManager.GetInstance().PlayerDied -= OnPlayerDied;
    }

    protected override void SetNewDirection()
    {
        switch (_objectMoveHandler)
        {
            case ObjectBasicMove:

                _direction = Vector2.up;
                break;

            case ObjectAdvancedMove:

                UpdatePoints();
                break;
        }
    }

    private void UpdatePoints()
    {
        _direction = _movePoints[_point];
        _point = (_point == 1) ? 0 : 1;
    }

    private void NewMovePoints()
    {
        for (int i = 0; i < _movePoints.Length; i++)
        {
            _movePoints[i] = GenerateMovePoint(_patrolAreas[i]);
        }

        _point = Random.Range(0, _movePoints.Length);
    }

    protected Vector3 GenerateMovePoint(Bounds area)
    {
        float randomX = Random.Range(area.min.x, area.max.x);
        float randomY = Random.Range(area.min.y, area.max.y);
        float Z = 0;

        return new Vector3(randomX, randomY, Z);
    }
}