using Zenject;
using UnityEngine;
using System;


namespace Refactoring
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private Vector3 _playerSpawnPosition;

        [SerializeField] private Player _prefab;
        [SerializeField] private DefenceDroneBehaviour[] _defenceDrones;
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private DefenceDroneData _defenceDroneData;
 
        [SerializeField] private int _healAmount;
        [SerializeField] private int _nukesToAdd;


        public override void InstallBindings()
        {
            BindPlayerData();

            BindBonusEffects();

            Container
                .BindInterfacesAndSelfTo<PlayerHealthBar>()
                .FromNew()
                .AsSingle()
                .WithArguments(_prefab.gameObject);

            BindWeapons();

            Container
                .BindInterfacesAndSelfTo<PlayerMover>()
                .WithArguments(_prefab.transform)
                .WhenInjectedInto<PlayerController>();

            Container
                .Bind<float>()
                .FromInstance(_playerData.Speed)
                .WhenInjectedInto<PlayerController>();

            Container
                .Bind<Player>()
                .FromInstance(_prefab)
                .AsSingle()
                .NonLazy();
        }

        private void BindWeapons()
        {
            var weaponTimer1 = new Timer(_prefab);
            var weaponTimer2 = new Timer(_prefab);

            Container
                .BindInterfacesAndSelfTo<PlayerCanon>()
                .AsCached()
                .WithArguments(_prefab.transform, weaponTimer1)
                .WhenInjectedInto<PlayerController>();

            Container
                .BindInterfacesAndSelfTo<PlayerTripleCanon>()
                .AsCached()
                .WithArguments(_prefab.transform, weaponTimer2)
                .WhenInjectedInto<PlayerController>();
        }

        private void BindBonusEffects()
        {
            Container
                 .BindInterfacesAndSelfTo<RepairBonus>()
                 .AsTransient()
                 .WithArguments(_healAmount);

            Container
                .BindInterfacesAndSelfTo<MultilaserBonus>()
                .AsTransient()
                .WithArguments(_playerData.BonusLenght);

            Container
                .BindInterfacesAndSelfTo<ForceFieldBonus>()
                .AsTransient()
                .WithArguments(_playerData.BonusLenght);

            Container
                .BindInterfacesAndSelfTo<NukeBonus>()
                .AsTransient()
                .WithArguments(_nukesToAdd);

            Container.BindInterfacesAndSelfTo<DroneBonus>().AsTransient();

            Container
                .BindInterfacesAndSelfTo<PlayerBonusHandler>()
                .FromNew()
                .AsSingle()
                .WithArguments(_prefab, _defenceDrones);
        }

        private void BindPlayerData()
        {
            var targets = new Type[]
                        {
            typeof(PlayerBonusHandler),
            typeof(PlayerCanon),
            typeof(PlayerTripleCanon),
            typeof(PlayerHealthBar),
            typeof(PlayerMover)
                        };

            Container
                .BindInterfacesAndSelfTo<PlayerData>()
                .FromInstance(_playerData)
                .AsSingle()
                .WhenInjectedInto(targets)
                .NonLazy();

            Container
                .BindInterfacesAndSelfTo<DefenceDroneData>()
                .FromInstance(_defenceDroneData)
                .AsSingle()
                .WhenInjectedInto<DefenceDroneBehaviour>()
                .NonLazy();
        }
    }
}
