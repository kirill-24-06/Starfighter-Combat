using UnityEngine;

//Performs all the logic of AdvancedBechaviour and BasicEnemyAttacker
public class AdvancedEnemy : AdvancedBehaviour
{
    private IDamageble _enemyHealthHandler;
    private IAttacker _enemyAttackHandler;

    private readonly float _activeAreaX = 19.0f;
    private readonly float _activeAreaLower = -3.0f;
    private readonly float _activeAreaUpper = 10.0f;

    [SerializeField] private int _shotsBeforePositionChange;
    private float _shotsFired = 0;

    private Transform _player;

    private void Awake()
    {
        Initialise();

        _enemyHealthHandler = new Damageble(this);
        _enemyAttackHandler = new MultipleCanonsAttacker(this);

        _player = EntryPoint.Player.transform;
    }

    protected void OnEnable()
    {
        _isArrived = false;

        ChangeMover(new ObjectBasicMove(this));
        _direction = Vector3.up;

        _enemyAttackHandler.Reset();
        _enemyHealthHandler.ResetHealth();
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

    private new void OnDisable()
    {
        base.OnDisable();
        _shotsFired = 0;
    }


    private void LookInTargetDirection(Vector3 target)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ObjectHolder.GetInstance().FindRegisteredObject(collision.gameObject, ObjectTag.PlayerWeapon))
        {
            _enemyHealthHandler.TakeDamage(1);
            ObjectPoolManager.ReturnObjectToPool(collision.gameObject);
        }
    }
}