using UnityEngine;

public class PlayerBehaviour : ObjectBehaviour
{
    private static PlayerBehaviour _instance;

    private PolygonCollider2D _playerCollider;
    private IAttacker _playerAttackHandler;
    private IDamageble _playerDamageHandler;
    private PlayerController _playerController;
    private BonusHandler _bonusHandler;

    private Timer _bonusTimer;
    private int _ionSpheresAmount = 0;
    private bool _isEquiped;
    private bool _bonusIsTaken = false;

    private bool _isInvunerable = false;

    private GameObject _projectile;

    [SerializeField] private GameObject[] _defenceDrones;
    private int _defenceDronesAmount = 0;
    private bool _isDroneActive;

    public bool IsTaken => _bonusIsTaken;

    public void Initialise()
    {
        _instance = this;
        _playerCollider = GetComponent<PolygonCollider2D>();
        _bonusTimer = new Timer(this);
        _objectMoveHandler = new PlayerMover(this);
        _playerAttackHandler = new Attacker(this);
        _playerDamageHandler = new Damageble(this);
        _playerController = new PlayerController();
        _bonusHandler = BonusHandler.GetInstance();

        _projectile = ObjectInfo.Projectile;

        EventManager.GetInstance().BonusCollected += OnBonusTake;
        EventManager.GetInstance().Multilaser += OnMultilaserEnable;
        EventManager.GetInstance().MultilaserEnd += OnMultilaserDisable;
        EventManager.GetInstance().Invunerable += Invunerability;
    }

    private void Update()
    {
        _objectMoveHandler.Move(_playerController.InputDirection(), ObjectInfo.Speed);
        CheckBorders();

        if (_playerController.ShootInput())
        {
            _playerAttackHandler.Fire(_projectile);
        }

        if (_playerController.BonusInput() && _isEquiped)
        {
            UseSpehre();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ObjectHolder.GetInstance().FindRegisteredObject(collision.gameObject, ObjectTag.EnemyWeapon) ||
            ObjectHolder.GetInstance().FindRegisteredObject(collision.gameObject, ObjectTag.Enemy))
        {
            if (!_isInvunerable && !_isDroneActive)
            {
                _playerDamageHandler.TakeDamage(1);
            }

            else if (_isDroneActive)
            {
                DestroyDrone();
            }

            ObjectPoolManager.ReturnObjectToPool(collision.gameObject);
        }
    }

    private void OnBonusTake(BonusTag bonusTag)
    {
        switch (bonusTag)
        {
            case BonusTag.Health:

                _playerDamageHandler.RecieveHealth(1);
                break;

            case BonusTag.Multilaser:

                _bonusHandler.ActivateMultilaser(_bonusTimer, 10);
                break;

            case BonusTag.LaserBeam:

                _bonusHandler.ActivateLaserBeam(_bonusTimer, 10);
                break;

            case BonusTag.ForceField:

                _bonusHandler.ActivateForceField(_bonusTimer, 10);
                break;

            case BonusTag.IonSphere:

                _ionSpheresAmount++;
                _isEquiped = _ionSpheresAmount > 0;
                break;

            case BonusTag.DefenceDrone:

                ActivateDrone();
                break;
        }
    }

    private void OnMultilaserEnable()
    {
        Debug.Log("MultilaserStart");
        _bonusIsTaken = true;
        _playerAttackHandler = new MultipleCanonsAttacker(this);
    }

    private void OnMultilaserDisable()
    {
        Debug.Log("MultilaserEnd");
        _bonusIsTaken = false;
        _playerAttackHandler = new Attacker(this);
        _bonusTimer.ResetTimer();
    }

    private void OnLaserBeamEnable()
    {
        Debug.Log("MultilaserStart");
        _bonusIsTaken = true;
        _playerAttackHandler = new MultipleCanonsAttacker(this);
    }

    private void OnLaserBeamDisableDisable()
    {
        Debug.Log("MultilaserEnd");
        _bonusIsTaken = false;
        _playerAttackHandler = new Attacker(this);
        _bonusTimer.ResetTimer();
    }

    private void UseSpehre()
    {
        _ionSpheresAmount--;
        _isEquiped = _ionSpheresAmount > 0;
        EventManager.GetInstance().IonSphereUse?.Invoke();
    }

    private void ActivateDrone()
    {
        if (_defenceDronesAmount < _defenceDrones.Length)
        {
            _defenceDronesAmount++;
            _isDroneActive = _defenceDronesAmount > 0;

            foreach (GameObject drone in _defenceDrones)
            {
                if (!drone.activeInHierarchy)
                {
                    drone.SetActive(true);
                    break;
                }
            }
        }
    }

    private void DestroyDrone()
    {
        _defenceDronesAmount--;
        _isDroneActive = _defenceDronesAmount > 0;

        for (int i = _defenceDrones.Length - 1; i >= 0; i--)
        {
            if (_defenceDrones[i].activeInHierarchy)
            {
                _defenceDrones[i].SetActive(false);
                break;
            }
        }
    }

    private void Invunerability(bool value)
    {
        _isInvunerable = value;
        _playerCollider.enabled = !_isInvunerable;
    }

    private void CheckBorders()
    {
        if (transform.position.x < -ObjectInfo.GameZoneBorders.x)
        {
            transform.position = new Vector3(-ObjectInfo.GameZoneBorders.x, transform.position.y, transform.position.z);
        }

        if (transform.position.x > ObjectInfo.GameZoneBorders.x)
        {
            transform.position = new Vector3(ObjectInfo.GameZoneBorders.x, transform.position.y, transform.position.z);
        }

        if (transform.position.y < -ObjectInfo.GameZoneBorders.y)
        {
            transform.position = new Vector3(transform.position.x, -ObjectInfo.GameZoneBorders.y, transform.position.z);
        }

        if (transform.position.y > ObjectInfo.GameZoneBorders.y)
        {
            transform.position = new Vector3(transform.position.x, ObjectInfo.GameZoneBorders.y, transform.position.z);
        }
    }

    public static bool IsPlayer(Collider2D collider)
    {
        return collider.gameObject == _instance.gameObject;
    }
}