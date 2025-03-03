using Zenject;
using UnityEngine;
using Utils.SpawnSystem;
using System.Collections.Generic;
using System;


namespace Refactoring
{
    public class SceneObjectsInstaller : MonoInstaller
    {
        [SerializeField] private AsteroidSettings _asteroidSettings;
        [SerializeField] private FighterSettings _fighterSettings;
        [SerializeField] private InterceptorSettings _interceptorSettings;
        [SerializeField] private RocketShipSettings _rocketShipSettings;
        [SerializeField] private BossDefenceDroneSettings _bossDefenceDroneSettings;

        [SerializeField] private AceSettings _aceSettings;

        [SerializeField] private BonusSettings[] _bonuses;

        public override void InstallBindings()
        {
            InstallUnits();
        }

        private void InstallUnits()
        {
            var keys = new List<MonoProduct>()
            {
                _asteroidSettings.Prefab,
                _fighterSettings.Prefab,
                _interceptorSettings.Prefab,
                _rocketShipSettings.Prefab,
                _bossDefenceDroneSettings.Prefab,
                _aceSettings.Prefab,
            };

            Container
                .Bind<Utils.SpawnSystem.IFactory<MonoProduct>>()
                .To<AsteroidFactory>()
                .AsCached()
                .WithArguments(_asteroidSettings)
                .WhenInjectedInto<Spawner>()
                .NonLazy();

            Container
               .Bind<Utils.SpawnSystem.IFactory<MonoProduct>>()
               .To<FighterFactory>()
               .AsCached()
               .WithArguments(_fighterSettings)
               .WhenInjectedInto<Spawner>()
               .NonLazy();

            Container
               .Bind<Utils.SpawnSystem.IFactory<MonoProduct>>()
               .To<InterceptorFactory>()
               .AsCached()
               .WithArguments(_interceptorSettings)
               .WhenInjectedInto<Spawner>()
               .NonLazy();

            Container
               .Bind<Utils.SpawnSystem.IFactory<MonoProduct>>()
               .To<RocketShipFactory>()
               .AsCached()
               .WithArguments(_rocketShipSettings)
               .WhenInjectedInto<Spawner>()
               .NonLazy();


            Container
               .Bind<Utils.SpawnSystem.IFactory<MonoProduct>>()
               .To<BossDefenceDroneFactory>()
               .AsCached()
               .WithArguments(_bossDefenceDroneSettings)
               .WhenInjectedInto<Spawner>()
               .NonLazy();

            Container
               .Bind<Utils.SpawnSystem.IFactory<MonoProduct>>()
               .To<EnemyAceFactory>()
               .AsCached()
               .WithArguments(_aceSettings)
               .WhenInjectedInto<Spawner>()
               .NonLazy();

            foreach (var bonus in _bonuses)
            {
                keys.Add(bonus.Prefab);

                Container
                    .Bind<Utils.SpawnSystem.IFactory<MonoProduct>>()
                    .To<BonusFactory>()
                    .AsCached()
                    .WithArguments(bonus)
                    .WhenInjectedInto<Spawner>()
                    .NonLazy();
            }

          


            Container.
                Bind<List<MonoProduct>>().
                FromInstance(keys).
                AsSingle().
                WhenInjectedInto<Spawner>().
                NonLazy();
        }
    }
}
