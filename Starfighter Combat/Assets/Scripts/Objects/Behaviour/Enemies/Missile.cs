using System.Collections.Generic;
using UnityEngine;


public class Missile : BasicBehaviour
{
    private ObjectHolder _targets;

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

        _targets = ObjectHolder.GetInstance();
        _objectMoveHandler = new ObjectBasicMove(this);

        _launchTimer.TimeIsOver += OnHomingStart;
        _homingTimer.TimeIsOver += OnHomingEnd;

        EventManager.GetInstance().IonSphereUse += OnIonSphereUse;
    }

    private void OnEnable()
    {
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

        DeactivateOutOfBounds();

        if (_target != null && !_target.gameObject.activeInHierarchy)
        {
            _target = null;
            _direction = Vector3.up;
        }
    }

    private void OnDisable()
    {
        _target = null;
        _homing = false;
    }

    private void OnHomingStart()
    {
        LockOnTarget();
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

    private void LockOnTarget()
    {
        if (ObjectInfo.Tag == ObjectTag.EnemyWeapon)
        {
            _target = EntryPoint.Instance.Player.transform;
        }

        else if (ObjectInfo.Tag == ObjectTag.PlayerWeapon)
        {
            SeekNearestEnemy();
        }
    }

    private void SeekNearestEnemy()
    {
        Transform nearestEnemy = null;

        List<GameObject> targets;
        float nearestEnemyDistance = Mathf.Infinity;

        targets = _targets.GetRegisteredObjectsByTag(ObjectTag.Enemy);

        foreach (GameObject target in targets)
        {
            if (target.activeInHierarchy)// Баг при рестарте уровня: 
            {
                float currdistance = Vector2.Distance(transform.position, target.transform.position);

                if (currdistance < nearestEnemyDistance)
                {
                    nearestEnemy = target.transform;

                    nearestEnemyDistance = currdistance;
                }
            }
        }

        _target = nearestEnemy;
    }

    private void OnDestroy()
    {
        _launchTimer.TimeIsOver -= OnHomingStart;
        _homingTimer.TimeIsOver -= OnHomingEnd;

        EventManager.GetInstance().IonSphereUse -= OnIonSphereUse;
    }
}