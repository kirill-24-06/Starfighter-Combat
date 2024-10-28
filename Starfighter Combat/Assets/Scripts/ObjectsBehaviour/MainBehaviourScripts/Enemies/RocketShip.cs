using UnityEngine;

public class RocketShip : Enemy
{
    [SerializeField] private RocketShipData _data;

    private Patrol _mover;
    private EnemyAdvancedAttacker _attacker;
    private Transform _target;

    private Timer _liveTimer;

    private AudioSource _audioPlayer;

    protected override void Awake()
    {
        base.Awake();
        Initialise();
    }

    private void Start()
    {
        var collider = GetComponent<Collider2D>();
        EntryPoint.Instance.CollisionMap.Register(collider, this);
        EntryPoint.Instance.CollisionMap.RegisterNukeInteractable(collider, this);
        EntryPoint.Instance.MissileTargets.AddEnemy(transform);
    }

    private void OnEnable()
    {
        _health = _data.Health;
        _attacker.Reset();
        _mover.Reset();
        _mover.NewMovePoints();
    }

    protected override void Update()
    {
        base.Update();

        Attack();
    }

    private void OnDisable()
    {
        //_attacker.Reset();
        //_mover.Reset();
        //_mover.NewMovePoints();
        StopAllCoroutines();
    }

    protected override void Initialise()
    {
        _mover = new Patrol(transform, _data);
        _target = EntryPoint.Instance.Player.transform;
        _attacker = new EnemyAdvancedAttacker(this, _data, _data.ShootsBeforeRetreat);

        _liveTimer = new Timer(this);
        _liveTimer.TimeIsOver += _mover.Disengage;

        _mover.Arrival += OnArrival;
        _attacker.AttackRunComplete += OnAttackRunComplete;

        _audioPlayer = EntryPoint.Instance.GlobalSoundFX;

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
            _attacker.Fire(_data.EnemyMissile.gameObject);
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
        ObjectPoolManager.SpawnObject(_data.Explosion, transform.position,
            _data.Explosion.transform.rotation, ObjectPoolManager.PoolType.ParticleSystem);

        ObjectPoolManager.ReturnObjectToPool(gameObject);

        _events.AddScore?.Invoke(_data.Score);
        _events.EnemyDestroyed?.Invoke(_data.EnemyStrenght);

        _audioPlayer.PlayOneShot(_data.ExplosionSound, _data.ExplosionSoundVolume);
    }
}
