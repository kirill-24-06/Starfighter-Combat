using Zenject;
using UnityEngine;
using Utils.SpawnSystem;
using System;

namespace Refactoring
{
    public class VisualEffectsInstaller : MonoInstaller
    {
        [SerializeField] private AnimationEffectSettings _collisionEffectSettings;

        [SerializeField] private AnimationEffectSettings _smallExplosionSettings;

        [SerializeField] private AnimationEffectSettings _explosionEffectsSettings;
        [SerializeField] private AnimationEffectSettings _nukeSettings;


        public override void InstallBindings()
        {
            var laserCollisionsTargets = new Type[]
            {
                typeof(PlayerLaserFactory),
                typeof(EnemyLaserFactory),
            };

            var smallExplosionTargets = new Type[]
            {
                typeof(PlayerLaserFactory),
                typeof(EnemyLaserFactory),
                typeof(PlayerMissileFactory),
                typeof(EnemyMissileFactory),
                typeof(DefenceDroneBehaviour)
            };

            var explosionEffectTargets = new Type[]
            {
                typeof(PlayerHealthBar),
                typeof(AsteroidFactory),
                typeof(FighterFactory),
                typeof(InterceptorFactory),
                typeof(RocketShipFactory),
                typeof(BossDefenceDroneFactory),
                typeof(EnemyAceFactory)
            };


            Container
                .Bind<Utils.SpawnSystem.IFactory<MonoProduct>>()
                .To<VisualEffectsFactory>()
                .AsCached()
                .WithArguments(_explosionEffectsSettings)
                .WhenInjectedInto(explosionEffectTargets)
                .NonLazy();

            Container
                .Bind<Utils.SpawnSystem.IFactory<MonoProduct>>()
                .WithId("ExplosionEffect")
                .To<VisualEffectsFactory>()
                .AsCached()
                .WithArguments(_smallExplosionSettings)
                .WhenInjectedInto(smallExplosionTargets)
                .NonLazy();

            Container
              .Bind<Utils.SpawnSystem.IFactory<MonoProduct>>()
              .WithId("CollisionEffect")
              .To<VisualEffectsFactory>()
              .AsCached()
              .WithArguments(_collisionEffectSettings)
              .WhenInjectedInto(laserCollisionsTargets)
              .NonLazy();

            Container
              .Bind<Utils.SpawnSystem.IFactory<MonoProduct>>()
              .To<VisualEffectsFactory>()
              .AsCached()
              .WithArguments(_nukeSettings)
              .WhenInjectedInto<PlayerBonusHandler>()
              .NonLazy();
        }
    }
}
