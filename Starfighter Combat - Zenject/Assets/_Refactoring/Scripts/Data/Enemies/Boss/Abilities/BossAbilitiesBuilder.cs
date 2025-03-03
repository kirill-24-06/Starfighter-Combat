using System;
using UnityEngine;
using Utils.SpawnSystem;
using static Refactoring.InterceptorWithAbilitiesStageSettings;

namespace Refactoring
{
    public class BossAbilitiesBuilder
    {
        private IFactory<MonoProduct> _missileFactory;

        private BossAbilitiesSettings _settings;

        private Transform _bossTransform;

        private BossShield _bossShield;

        public BossAbilitiesBuilder([Zenject.Inject(Id = "BossMissile")] IFactory<MonoProduct> missileFactory)
        {
            _missileFactory = missileFactory;
        }

        public void Initialize(BossAbilitiesSettings settings, Transform bossTransform)
        {
            _settings = settings;
            _bossTransform = bossTransform;
            _bossShield = _bossTransform.Find("BossShield").GetComponent<BossShield>();
        }

        public IBossAbility Build(Ability ability)
        {
            return ability switch
            {
                Ability.MissileLaunch => new BossMissileLaunchAbility(_bossTransform, _missileFactory),
                Ability.SpawnMinion => new SpawnMinionAbility(_settings.Minions),
                Ability.Shield => new BossShieldAbility(_bossShield),
                _ => throw new NotImplementedException(),
            };
        }

    }
}