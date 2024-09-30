using UnityEngine;

public abstract class Missile : BasicObject
{
    [SerializeField] protected MissileData _data;

    protected Transform _target;
    private Vector3 _direction;

    private IMover _mover;

    private Timer _launchTimer;

    private Timer _homingTimer;

    private bool _homing = false;

    protected virtual void Awake()
    {
        _launchTimer = new Timer(this);
        _homingTimer = new Timer(this);
    }

    protected void OnEnable()
    {
        _mover = new Mover(transform);
        _direction = Vector3.up;

        _launchTimer.TimeIsOver += OnHomingStart;
        _homingTimer.TimeIsOver += OnHomingEnd;

        EntryPoint.Instance.Events.IonSphereUse += OnIonSphereUse;

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
            _mover.Move(_target.transform.position, _data.Speed);

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

        EntryPoint.Instance.Events.IonSphereUse -= OnIonSphereUse;
    }

    protected virtual void OnHomingStart()
    {
        if (_target != null)
        {
            _mover = new MissileMover(transform);
            _homing = true;

            _homingTimer.SetTimer(_data.HomingTime);
            _homingTimer.StartTimer();
        }
    }

    private void OnHomingEnd()
    {
        _mover = new Mover(transform);

        _homing = false;
        _direction = Vector3.up;
    }

    private void OnIonSphereUse()
    {
        if (gameObject.activeInHierarchy)
            Disable();
    }

    private void DeactivateOutOfBounds()
    {
        if (transform.position.y < -_data.GameZoneBorders.y)
        {
            Disable();
        }

        if (transform.position.y > _data.GameZoneBorders.y)
        {
            Disable();
        }

        if (transform.position.x < -_data.GameZoneBorders.x)
        {
            Disable();
        }

        if (transform.position.x > _data.GameZoneBorders.x)
        {
            Disable();
        }
    }

    //protected void OnDestroy()
    //{
    //    _launchTimer.TimeIsOver -= OnHomingStart;
    //    _homingTimer.TimeIsOver -= OnHomingEnd;

    //    EntryPoint.Instance.Events.IonSphereUse -= OnIonSphereUse;
    //}
}
