using UnityEngine;
using Utils.Events.Channel.Static;
using Zenject;

namespace Refactoring
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private ForceFieldBehaviour _forceField;

        private static GameObject _playerGO;

        private PlayerBonusHandler _bonusHandler;

        private IDamageble _damageHandler;
        private IHealable _healthHandler;

        public bool IsInvunerable => _bonusHandler.IsInvunerable;
        public bool IsDroneActive => _bonusHandler.IsDroneActive;
        public ForceFieldBehaviour ForceField => _forceField;

        [Inject]
        public void Construct(IDamageble damageble, IHealable healable, PlayerBonusHandler bonusHandler)
        {
            _playerGO = gameObject;

            _damageHandler = damageble;
            _healthHandler = healable;

            _bonusHandler = bonusHandler;
        }

        private void Awake()
        {
            Channel<PlayerDiedEvent>.OnEvent += OnPlayerDied;
            Channel<BonusCollectedEvent>.OnEvent += ActivateBonus;
            Channel<PlayerDamagedEvent>.OnEvent += OnDamageTake;
            Channel<PlayerHealedEvent>.OnEvent += OnHealing;
        }

        public void Start() => _bonusHandler.OnStart();

        public void ActivateBonus(BonusCollectedEvent @event) => _bonusHandler.Handle(@event.BonusTag);

        private void OnDamageTake(PlayerDamagedEvent @event)
        {
            if (IsInvunerable) return;

            _damageHandler.TakeDamage(@event.Damage);
        }

        private void OnHealing(PlayerHealedEvent @event) => _healthHandler.Heal(@event.Health);

        private void OnPlayerDied(PlayerDiedEvent @event) => _playerGO.SetActive(false);

        public static bool IsPlayer(GameObject gameObject) => gameObject == _playerGO;

        public void OnDestroy()
        {
            Channel<PlayerDiedEvent>.OnEvent -= OnPlayerDied;
            Channel<BonusCollectedEvent>.OnEvent -= ActivateBonus;
            Channel<PlayerDamagedEvent>.OnEvent -= OnDamageTake;
            Channel<PlayerHealedEvent>.OnEvent -= OnHealing;
        }
    }
}
