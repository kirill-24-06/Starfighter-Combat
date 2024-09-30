
using UnityEngine;

public class Interceptor : Enemy
{
    [SerializeField] private InterceptorData _data;

    private AdvancedMove _mover;
    private EnemyAdvancedAttacker _attacker;
    private Transform _target;

    private Timer _liveTimer;


    protected override void Awake()
    {
        base.Awake();
        Initialise();
    }


    protected override void OnEnable()
    {
        base.OnEnable();
        _health = _data.Health;
    }


    protected override void Update()
    {
        base.Update();

        Attack();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _attacker.Reset();
        _mover.Reset();
        StopAllCoroutines();
    }

    protected override void Initialise()
    {
        _mover = new AdvancedMove(transform, _data.EngageZone);
        _target = EntryPoint.Instance.Player.transform;
        _attacker = new EnemyAdvancedAttacker(this, _data.ReloadCountDown, _data.ShootsBeforeRetreat);

        _liveTimer = new Timer(this);
        _liveTimer.TimeIsOver += _mover.Disengage;

        _mover.Arrival += OnArrival;
        _attacker.AttackRunComplete += OnAttackRunComplete;

        Data = _data;
    }
    protected override void Move()
    {
        _mover.Move(_data.Speed);

        DeactivateOutOfBounds(_data.DisableBorders);
    }

    private void Attack()
    {
        if (!_mover.IsMoving && _target.gameObject.activeInHierarchy)
        {
            _mover.LookInTargetDirection(_target.position);
            _attacker.Fire(_data.EnemyProjectile.gameObject);
        }
    }

    private void OnAttackRunComplete()
    {
        _mover.SetNewDirection();
    }

    private void OnArrival()
    {
        _liveTimer.SetTimer(_data.LiveTime);
        _liveTimer.StartTimer();
    }

    protected override void Disable()
    {
        Instantiate(_data.Explosion, transform.position, _data.Explosion.transform.rotation);

        ObjectPoolManager.ReturnObjectToPool(gameObject);

        _events.AddScore?.Invoke(_data.Score);
        _events.EnemyDestroyed?.Invoke(_data.EnemyStrenght);
    }
}
