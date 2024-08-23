using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player _instance;

    [SerializeField] private PlayerData _playerData;
    [SerializeField] private GameObject[] _defenceDrones;

    private EventManager _events;
    private PlayerController _controller;
    private PolygonCollider2D _playerCollider;
    private Timer _bonusTimer;

    private int _playerHealth;

    private int _ionSpheresAmount = 0;
    private int _defenceDronesAmount = 0;

    private bool _bonusIsTaken = false;
    private bool _isInvunerable = false;
    private bool _isDroneActive;
    private bool _isEquiped;

    public PlayerData PlayerData => _playerData;
    public bool IsTaken => _bonusIsTaken;
    public bool IsInvunerable => _isInvunerable;
    public bool IsEquiped => _isEquiped;
    public bool IsDroneActive => _isDroneActive;

    public Timer BonusTimer => _bonusTimer;


    public void Initialise()
    {
        _instance = this;
        _controller = GetComponent<PlayerController>();
        _playerCollider = GetComponent<PolygonCollider2D>();
        _bonusTimer = new Timer(this);

        _events = EventManager.GetInstance();

        _events.Start += OnStart;

        _events.PlayerDamaged += OnDamage;
        _events.PlayerHealed += OnHeal;
        _events.PlayerDied += OnPlayerDied;

        _events.BonusCollected += OnBonusTake;
        _events.MultilaserEnd += OnMultilaserEnd;
        _events.IonSphereUse += OnIonSphereUse;
        _events.Invunerable += OnForceFieldActive;
        _events.DroneDestroyed += OnDroneDestruction;

        _controller.Initialise();
    }

    private void OnStart()
    {
        _playerHealth = PlayerData.Health;
        _ionSpheresAmount = PlayerData.IonSpheresStartAmount;
        _isEquiped = _ionSpheresAmount > 0;

        _events.ChangeHealth?.Invoke(_playerHealth);
        _events.BonusAmountUpdate?.Invoke(_ionSpheresAmount);
    }

    private void OnDamage(int damage)
    {
        if (_isInvunerable)
            return;

        _playerHealth -= damage;

        if (_playerHealth < 0)
        {
            _playerHealth = 0;
        }

        _events.ChangeHealth?.Invoke(_playerHealth);

        if (_playerHealth == 0)
        {
            _events.PlayerDied?.Invoke();
        }
    }

    private void OnHeal(int heal)
    {
        _playerHealth += heal;

        if (_playerHealth > PlayerData.MaxHealth)
        {
            _playerHealth = PlayerData.MaxHealth;
            _events.AddScore?.Invoke(50);
        }

        _events.ChangeHealth?.Invoke(_playerHealth);
    }

    private void OnPlayerDied()
    {
        gameObject.SetActive(false);
    }

    private void OnBonusTake(BonusTag bonusTag)
    {
        switch (bonusTag)
        {
            case BonusTag.Health:

                OnHeal(1);
                break;

            case BonusTag.Multilaser:

                EnableMultilaser();
                break;

            case BonusTag.LaserBeam:

                //ToDo
                break;

            case BonusTag.ForceField:

                ActivateForceField();
                break;

            case BonusTag.IonSphere:

                AddIonSphere(1);
                break;

            case BonusTag.DefenceDrone:

                ActivateDrone();
                break;
        }
    }

    private void EnableMultilaser()
    {
        _bonusIsTaken = true;

        _events.Multilaser?.Invoke(true);

        _bonusTimer.SetTimer(_playerData.BonusTimeLenght);
        _bonusTimer.TimeIsOver += _events.MultilaserEnd;
        _bonusTimer.StartTimer();
    }

    private void OnMultilaserEnd()
    {
        _bonusIsTaken = false;
        _events.Multilaser?.Invoke(false);

        _bonusTimer.ResetTimer();
    }

    private void AddIonSphere(int amount)
    {
        _ionSpheresAmount += amount;
        _isEquiped = _ionSpheresAmount > 0;
        EventManager.GetInstance().BonusAmountUpdate?.Invoke(_ionSpheresAmount);
    }

    private void OnIonSphereUse()
    {
        _ionSpheresAmount--;
        _isEquiped = _ionSpheresAmount > 0;

        _events.BonusAmountUpdate?.Invoke(_ionSpheresAmount);
    }

    private void ActivateForceField()
    {
        _events.ForceField?.Invoke();

        _bonusTimer.SetTimer(_playerData.BonusTimeLenght);
        _bonusTimer.TimeIsOver += _events.ForceFieldEnd;
        _bonusTimer.StartTimer();
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

    private void OnDroneDestruction()
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

    private void OnForceFieldActive(bool value)
    {
        _isInvunerable = value;
        _playerCollider.enabled = !_isInvunerable;

        if (!value)
            _bonusTimer.ResetTimer();
    }

    public static bool IsPlayer(Collider2D collider)
    {
        return collider.gameObject == _instance.gameObject;
    }

    private void OnDestroy()
    {
        _events.Start -= OnStart;
        _events.PlayerDamaged -= OnDamage;
        _events.PlayerHealed -= OnHeal;
        _events.PlayerDied -= OnPlayerDied;

        _events.BonusCollected -= OnBonusTake;
        _events.MultilaserEnd -= OnMultilaserEnd;
        _events.IonSphereUse -= OnIonSphereUse;
        _events.Invunerable -= OnForceFieldActive;
        _events.DroneDestroyed -= OnDroneDestruction;
    }
}