using UnityEngine;

public abstract class Missile : BasicObject,INukeInteractable
{
    [SerializeField] protected MissileData _data;

    protected Transform _target;
    private Vector3 _direction;

    private IMover _mover;
    private Mover _forwardMover;
    private MissileMover _homingMover;

    private Timer _launchTimer;

    private Timer _homingTimer;

    private bool _homing = false;

    protected virtual void Awake()
    {
        _launchTimer = new Timer(this);
        _homingTimer = new Timer(this);

        _forwardMover = new Mover(transform);
        _homingMover = new MissileMover(transform);
    }

    //protected override void Start()
    //{
    //    base.Start();
    //    EntryPoint.Instance.CollisionMap.RegisterNukeInteractable(GetComponent<Collider2D>(), this);
    //}

    protected void OnEnable()
    {
        _mover = _forwardMover;
        _direction = Vector3.up;

        _launchTimer.TimeIsOver += OnHomingStart;
        _homingTimer.TimeIsOver += OnHomingEnd;

        _launchTimer.SetTimer(_data.LaunchTime);
        _launchTimer.StartTimer();
    }

    protected void Update()
    {
        Move();
        DeactivateOutOfBounds();

        if (_target != null && !_target.gameObject.activeInHierarchy)
        {
            _homingTimer.StopTimer();
            OnHomingEnd();
        }
    }

    protected override void Move()
    {
        if (_homing && _target != null)
            _mover.Move(_target.position, _data.Speed);

        else
            _mover.Move(_direction, _data.Speed);
    }

    protected override void Disable()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    protected void OnDisable()
    {
        _target = null;
        _homing = false;

        StopAllCoroutines();

        _launchTimer.TimeIsOver -= OnHomingStart;
        _homingTimer.TimeIsOver -= OnHomingEnd;
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

    //private void OnIonSphereUse()
    //{
    //    if (gameObject.activeInHierarchy)
    //        Disable();
    //}

    public void GetDamagedByNuke() => Disable();
   
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