using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    [SerializeField] private ForceFieldBehaviour _forceField;

    private static GameObject _playerGO;

    private EventManager _events;

    private PlayerBonusHandler _bonusHandler;

    private IDamageble _damageHandler;
    private IHealable _healthHandler;

    public bool IsInvunerable => _bonusHandler.IsInvunerable;
    public bool IsDroneActive => _bonusHandler.IsDroneActive;
    public ForceFieldBehaviour ForceField => _forceField;

    [Inject]
    public void Construct(IDamageble damageble, IHealable healable, PlayerBonusHandler bonusHandler, EventManager events)
    {
        _playerGO = gameObject;

        _events = events;

        _damageHandler = damageble;
        _healthHandler = healable;

        _bonusHandler = bonusHandler;

        _events.PlayerDied += OnPlayerDied;
        _events.BonusCollected += ActivateBonus;
        _events.PlayerDamaged += OnDamageTake;
        _events.PlayerHealed += OnHealing;
    }

    public void Start() => _bonusHandler.OnStart();

    public void ActivateBonus(BonusTag tag) => _bonusHandler.Handle(tag);

    private void OnDamageTake(int damage)
    {
        if (IsInvunerable) return;

        _damageHandler.TakeDamage(damage);
    }

    private void OnHealing(int healthAmount) => _healthHandler.Heal(healthAmount);

    private void OnPlayerDied() => _playerGO.SetActive(false);

    public static bool IsPlayer(GameObject gameObject) => gameObject == _playerGO;

    public void OnDestroy()
    {
        _events.PlayerDied -= OnPlayerDied;
        _events.BonusCollected -= ActivateBonus;
        _events.PlayerDamaged -= OnDamageTake;
        _events.PlayerHealed -= OnHealing;
    }
}