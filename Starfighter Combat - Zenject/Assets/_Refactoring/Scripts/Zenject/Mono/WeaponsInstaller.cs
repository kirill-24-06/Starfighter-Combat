using Zenject;
using UnityEngine;
using System;
using Utils.SpawnSystem;


namespace Refactoring
{
    public class WeaponsInstaller : MonoInstaller
    {
        [Header("Player")]
        [SerializeField] private PlayerLaserSettings _laserSettings;
        [SerializeField] private PlayerMissileSettings _missileSettings;

        [Header("Enemies")]
        [SerializeField] private EnemyLaserSettings _enemyLaserSettings;
        [SerializeField] private EnemyMissileSettings _enemyMissileSettings;

        [Header("Boss")]
        [SerializeField] private EnemyMissileSettings _bossMissileSettings;

        public override void InstallBindings()
        {

            Container
                .Bind<Utils.SpawnSystem.IFactory<MonoProduct>>()
                .WithId("PlayerProjectile")
                .To<PlayerLaserFactory>()
                .AsCached()
                .WithArguments(_laserSettings)
                .NonLazy();


            Container
              .Bind<Utils.SpawnSystem.IFactory<MonoProduct>>()
              .WithId("PlayerMissile")
              .To<PlayerMissileFactory>()
              .AsCached()
              .WithArguments(_missileSettings)
              .NonLazy();


            Container
              .Bind<Utils.SpawnSystem.IFactory<MonoProduct>>()
              .WithId("EnemyLaser")
              .To<EnemyLaserFactory>()
              .AsCached()
              .WithArguments(_enemyLaserSettings)
              .WhenInjectedInto(new Type[]
              {
                  typeof(FighterFactory),
                  typeof(InterceptorFactory),
                  typeof(InterceptorStageBuilder),
                  typeof(InterceptorWithAbilitiesStageBuilder)
              })
              .NonLazy();


            Container
             .Bind<Utils.SpawnSystem.IFactory<MonoProduct>>()
             .WithId("EnemyMissile")
             .To<EnemyMissileFactory>()
             .AsCached()
             .WithArguments(_enemyMissileSettings)
             .WhenInjectedInto<RocketShipFactory>()
             .NonLazy();


            Container
            .Bind<Utils.SpawnSystem.IFactory<MonoProduct>>()
            .WithId("BossMissile")
            .To<EnemyMissileFactory>()
            .AsCached()
            .WithArguments(_bossMissileSettings)
            .WhenInjectedInto(new Type[]
            {
                typeof(BossDefenceDroneFactory),
                typeof(BossAbilitiesBuilder),
            })
            .NonLazy();
        }
    }
}
