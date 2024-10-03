using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player _instance;

    [SerializeField] private PlayerData _playerData;
    [SerializeField] private DefenceDroneBehaviour[] _defenceDrones;
    [SerializeField] private GameObject _ionSphere;

    private EventManager _events;
    private PlayerController _controller;
    private PolygonCollider2D _playerCollider;
    private AudioSource _audioSource;
    private Timer _bonusTimer;

    [SerializeField]private ForceFieldBehaviour _forceField;
    [SerializeField] private GameObject _nukePoint;

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

    public ForceFieldBehaviour ForceField => _forceField;

    public Timer BonusTimer => _bonusTimer;


    public void Initialise()
    {
        _instance = this;
        _controller = GetComponent<PlayerController>();
        _playerCollider = GetComponent<PolygonCollider2D>();
        _audioSource = EntryPoint.Instance.GlobalSoundFX;
        _bonusTimer = new Timer(this);

        _events = EntryPoint.Instance.Events;

        _forceField.Initialise();

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

        else
        {
            EntryPoint.Instance.SpawnController.RespawnPlayer();
            gameObject.SetActive(false);
        }

        Instantiate(_playerData.Explosion, transform.position, _playerData.Explosion.transform.rotation);
        _audioSource.PlayOneShot(_playerData.ExplosionSound, _playerData.ExplosionSoundVolume);
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

        EntryPoint.Instance.HudManager.ActivateBonusTimer();
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
        EntryPoint.Instance.Events.BonusAmountUpdate?.Invoke(_ionSpheresAmount);
    }

    private void OnIonSphereUse()
    {
        Instantiate(_ionSphere, _nukePoint.transform.position, _ionSphere.transform.rotation);

        _ionSpheresAmount--;
        _isEquiped = _ionSpheresAmount > 0;

        _events.BonusAmountUpdate?.Invoke(_ionSpheresAmount);
    }

    public void ActivateForceField()
    {
        _events.ForceField?.Invoke();

        _bonusTimer.SetTimer(_playerData.BonusTimeLenght);
        _bonusTimer.TimeIsOver += _events.ForceFieldEnd;
        _bonusTimer.StartTimer();

        EntryPoint.Instance.HudManager.ActivateBonusTimer();
    }

    private void ActivateDrone()
    {
        if (_defenceDronesAmount < _defenceDrones.Length)
        {
            _defenceDronesAmount++;
            _isDroneActive = _defenceDronesAmount > 0;

            foreach (DefenceDroneBehaviour drone in _defenceDrones)
            {
                if (!drone.gameObject.activeInHierarchy)
                {
                    drone.gameObject.SetActive(true);
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
            if (_defenceDrones[i].gameObject.activeInHierarchy)
            {
                Instantiate(_defenceDrones[i].Explosion, _defenceDrones[i].gameObject.transform.position, _defenceDrones[i].Explosion.transform.rotation);
                _defenceDrones[i].gameObject.SetActive(false);
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

    public static bool IsPlayer(GameObject gameObject)
    {
        return gameObject.gameObject == _instance.gameObject;
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