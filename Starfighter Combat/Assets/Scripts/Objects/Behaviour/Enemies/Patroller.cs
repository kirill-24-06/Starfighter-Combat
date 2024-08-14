using UnityEngine;

public class Patroller : AdvancedEnemy
{
    private void Awake()
    {
        Initialise();

        _enemyHealthHandler = new Damageble(this);
        _enemyAttackHandler = new MultipleCanonsAttacker(this);

        EventManager.GetInstance().IonSphereUse += OnIonSphereUse;
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
        _player = EntryPoint.Player.transform;
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
            _shotsFired += 1 * Time.deltaTime;
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
    }

    protected override Vector3 GenerateMovePoint()
    {
        //float positionX = Random.Range(-_activeAreaX, _activeAreaX);
        //float positionY = Random.Range(_activeAreaLower, _activeAreaUpper);

        return base.GenerateMovePoint();
    }
}
