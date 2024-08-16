using UnityEngine;


public class Missile : BasicBehaviour
{
    private Transform _target;
    private Vector3 _direction;

    private Timer _launchTimer;
    [SerializeField] private float _launchTime = 3.0f;

    private Timer _homingTimer;
    [SerializeField] private float _homingTime = 5.0f;

    private bool _homing = false;

    private void Awake()
    {
        _launchTimer = new Timer(this);
        _homingTimer = new Timer(this);

        _objectMoveHandler = new ObjectBasicMove(this);

        _launchTimer.TimeIsOver += OnHomingStart;
        _homingTimer.TimeIsOver += OnHomingEnd;

        EventManager.GetInstance().IonSphereUse += OnIonSphereUse;
    }

    private void OnEnable()
    {
        _target = EntryPoint.Player.transform;
        _direction = Vector3.up;

        _launchTimer.SetTimer(_launchTime);
        _launchTimer.StartTimer();
    }

    private void Update()
    {
        if (_homing && _target != null)
        {
            _direction = (_target.transform.position - transform.position).normalized;

            transform.position += ObjectInfo.Speed * Time.deltaTime * _direction;
            LookInTargetDirection(_target.position);
        }

        else
        {
            _objectMoveHandler.Move(_direction, ObjectInfo.Speed);
        }

        if (_target != null && !_target.gameObject.activeInHierarchy)
        {
            _target = null;
        }
    }

    private void OnDisable()
    {
        _target = null;
        _homing = false;
    }

    private void OnHomingStart()
    {
        _homing = true;

        _homingTimer.SetTimer(_homingTime);
        _homingTimer.StartTimer();
    }

    private void OnHomingEnd()
    {
        _homing = false;
        _direction = Vector3.up;
    }

    private void OnIonSphereUse()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    protected void LookInTargetDirection(Vector3 target)
    {
        float rotation = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg - 90;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, rotation);
    }
}