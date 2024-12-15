using UnityEngine;

public abstract class Missile : MonoBehaviour, INukeInteractable
{
    [SerializeField] protected MissileData _data;
    [SerializeField] protected GameObject _explosionPrefab;

    protected GameObject _gameObject;
    protected Transform _transform;

    protected Transform _target;
    private Vector3 _direction;

    private IMover _mover;
    private Mover _forwardMover;
    private MissileMover _homingMover;

    private Timer _launchTimer;

    private Timer _homingTimer;
    private bool _homing = false;

    protected EventManager _events;
    protected bool _isPooled = true;

    protected virtual void Awake()
    {
        _events = EntryPoint.Instance.Events;

        _gameObject = gameObject;
        _transform = transform;

        _launchTimer = new Timer(this);
        _homingTimer = new Timer(this);

        _launchTimer.TimeIsOver += OnHomingStart;
        _homingTimer.TimeIsOver += OnHomingEnd;

        _forwardMover = new Mover(transform);
        _homingMover = new MissileMover(transform);

        PoolRootMap.SetParrentObject(_gameObject, GlobalConstants.PoolTypesByTag[_data.Tag]);
    }

    protected void OnEnable()
    {
        _mover = _forwardMover;
        _direction = Vector3.up;

        _launchTimer.SetTimer(_data.LaunchTime);
        _launchTimer.StartTimer();

        _isPooled = false;
    }

    protected void Update()
    {
        Move();

        if (_target != null && !_target.gameObject.activeInHierarchy)
        {
            _homingTimer.StopTimer();
            OnHomingEnd();
        }
    }

    private void Move()
    {
        if (_homing && _target != null)
            _mover.Move(_target.position, _data.Speed);

        else
            _mover.Move(_direction, _data.Speed);
    }

    protected void OnDisable()
    {
        _target = null;
        _homing = false;

        _launchTimer.StopTimer();
        _homingTimer.StopTimer();
    }

    protected virtual void OnHomingStart()
    {
        if (_target != null)
        {
            _mover = _homingMover;
            _homing = true;

            _homingTimer.SetTimer(_data.HomingTime);
            _homingTimer.StartTimer();
        }
    }

    private void OnHomingEnd()
    {
        _mover = _forwardMover;

        _homing = false;
        _direction = Vector3.up;
    }

    public void GetDamagedByNuke()
    {
        if (_isPooled) return;
        _isPooled = true;

        ObjectPool.Get(_explosionPrefab, _transform.position, _explosionPrefab.transform.rotation);

        ObjectPool.Release(_gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isPooled) return;
        _isPooled = true;

        ObjectPool.Release(_gameObject);
    }
}