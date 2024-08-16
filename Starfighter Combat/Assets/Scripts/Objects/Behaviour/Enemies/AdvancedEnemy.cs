using UnityEngine;

//Performs all the logic of AdvancedBechaviour and BasicEnemyAttacker
public class AdvancedEnemy : AdvancedBehaviour
{
    protected IDamageble _enemyHealthHandler;
    protected IAttacker _enemyAttackHandler;

    protected readonly float _activeAreaX = 19.0f;
    protected readonly float _activeAreaLower = -3.0f;
    protected readonly float _activeAreaUpper = 10.0f;

    [SerializeField] protected int _shotsBeforePositionChange;
    protected float _shotsFired = 0;

    protected Transform _player;

    private void Awake()
    {
        Initialise();

        _enemyHealthHandler = new Damageble(this);
        _enemyAttackHandler = new MultipleCanonsAttacker(this);

        EventManager.GetInstance().IonSphereUse += OnIonSphereUse;
        EventManager.GetInstance().Fire += OnFire;
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
        }

        if (_shotsFired == _shotsBeforePositionChange)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ObjectHolder.GetInstance().FindRegisteredObject(collision.gameObject, ObjectTag.PlayerWeapon))
        {
            _enemyHealthHandler.TakeDamage(1);
            ObjectPoolManager.ReturnObjectToPool(collision.gameObject);
        }
    }

    protected void OnFire(ObjectBehaviour enemy)
    {
        if (enemy == this)
        {
            _shotsFired++;
        }
    }


    protected void OnIonSphereUse()
    {
        if (gameObject.activeInHierarchy)
        {
            _enemyHealthHandler.TakeDamage(10);
        }
    }

    protected void LookInTargetDirection(Vector3 target)
    {
        float rotation = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg - 90;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, rotation);
    }

    protected override void SetNewDirection()
    {
        switch (_objectMoveHandler)
        {
            case ObjectBasicMove:

                _direction = Vector2.up;
                break;

            case ObjectAdvancedMove:

                _direction = GenerateMovePoint();
                LookInTargetDirection(_direction);
                break;
        }
    }

    protected override Vector3 GenerateMovePoint()
    {
        float positionX = Random.Range(-_activeAreaX, _activeAreaX);
        float positionY = Random.Range(_activeAreaLower, _activeAreaUpper);

        return new Vector3(positionX, positionY, 0);
    }
}