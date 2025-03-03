using System.Collections.Generic;
using UnityEngine;
using Utils.Events.Channel.Static;
using Utils.SpawnSystem;
using Utils.StateMachine;

namespace Refactoring
{
    public class EnemyAce : Boss
    {
        [SerializeField] private BossShield _bossShield;
        [field: SerializeField] public GameObject TransitionShield { get; private set; }

        private AceSettings _aceSettings;

        private bool _isInvunerable;

        public void Construct(
            AceSettings settings,
            StateMachine bossStateMachine,
            BossHealthComponent healthComponent,
            List<IResetable> resetables,
            IFactory<MonoProduct> explosionEffectsFactory,
            Player player,
            CollisionMap collisionMap)
        {
            _aceSettings = settings;
            _enemyData = _aceSettings;
            _bossData = _aceSettings;

            _bossStateMachine = bossStateMachine;

            _damageHandler = healthComponent;
            healthComponent.Dead += OnDead;

            Channel<BossInvunrableEvent>.OnEvent += OnBossInvunrable;

            _resetables = resetables;

            _explosionEffectsFactory = explosionEffectsFactory;

            _player = player;

            _collisionMap = collisionMap;

            _bossShield.Initialize(player, collisionMap);

            _explosionSound = new PlayGlobalSoundEvent(_aceSettings.ExplosionSound, _aceSettings.ExplosionSoundVolume);

            IsInPool = false;

            IsConstructed = true;
        }

        private void OnBossInvunrable(BossInvunrableEvent @event) => _isInvunerable = @event.Value;

        protected override void TakeDamage(int damage)
        {
            if (_bossShield.IsActive || _isInvunerable) return;

            _damageHandler.TakeDamage(damage);
        }

        protected override void OnDead()
        {
            if (IsInPool) return;
            IsInPool = true;

            CreateExplosion();

            Channel<BossDefeatEvent>.Raise(new BossDefeatEvent());

            Channel<PlayGlobalSoundEvent>.Raise(_explosionSound);

            Release();
        }

        private void OnDestroy()
        {
            Channel<BossInvunrableEvent>.OnEvent -= OnBossInvunrable;
        }
    }
}