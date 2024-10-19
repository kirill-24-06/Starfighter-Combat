using Cysharp.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player _instance;

    [SerializeField] private PlayerData _playerData;
    [SerializeField] private ForceFieldBehaviour _forceField;

    private EventManager _events;
    private PlayerController _controller;
    private PolygonCollider2D _playerCollider;
    private SpriteRenderer _spriteRenderer;
    private PlayerBonusHandler _bonusHandler;

    private IResetable _healthResetHandler;
    private Color _tempInvunrabilityColor = new Color(1, 1, 1, 0.2f);

    private bool _isInvunerable = false;

    public PlayerData PlayerData => _playerData;
    public bool IsActive => _bonusHandler.BonusIsActive;
    public bool IsInvunerable => _isInvunerable;
    public bool IsEquiped => _bonusHandler.IsEquiped;
    public bool IsDroneActive => _bonusHandler.IsDroneActive;
    public ForceFieldBehaviour ForceField => _forceField;
    public Timer BonusTimer => _bonusHandler.BonusTimer;

    public void Initialise()
    {
        _instance = this;
        _controller = GetComponent<PlayerController>();
        _playerCollider = GetComponent<PolygonCollider2D>();
        _spriteRenderer = transform.Find("Texture").GetComponent<SpriteRenderer>();

        PlayerHealthHandler playerHealth = new(this);
        _healthResetHandler = playerHealth;

        _bonusHandler = new PlayerBonusHandler(this);

        _events = EntryPoint.Instance.Events;

        _forceField.Initialise();

        _events.Stop += OnStop;
        _events.PlayerDied += OnPlayerDied;
        _events.Invunerable += OnForceFieldActive;

        _controller.Initialise();
    }

    private void Start()
    {
        _events.ChangeHealth?.Invoke(PlayerData.Health);
        _bonusHandler.OnStart();
    }
    
    private void OnStop() => _isInvunerable = true;

    private void OnPlayerDied() => gameObject.SetActive(false);

    public async UniTaskVoid StartTempInvunrability()
    {
        _isInvunerable = true;
        _spriteRenderer.color = _tempInvunrabilityColor;

        await UniTask.Delay(_playerData.TempInvunrabilityTimeMilliseconds, cancellationToken: destroyCancellationToken);

        _spriteRenderer.color = Color.white;

        if (!_forceField.ShieldActive)
            _isInvunerable = false;
    }

    private void OnForceFieldActive(bool value)
    {
        _isInvunerable = value;
        _playerCollider.enabled = !_isInvunerable;

        if (!value)
            BonusTimer.ResetTimer();
    }

    public static bool IsPlayer(GameObject gameObject)
    {
        return gameObject.gameObject == _instance.gameObject;
    }

    private void OnDestroy()
    {
        _events.Stop -= OnStop;
        _events.PlayerDied -= OnPlayerDied;
        _events.Invunerable -= OnForceFieldActive;

        _bonusHandler.Reset();
        _healthResetHandler.Reset();
    }
}