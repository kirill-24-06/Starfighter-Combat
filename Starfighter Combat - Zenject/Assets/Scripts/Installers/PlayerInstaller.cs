using System;
using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private Player _player;
    [SerializeField] private DefenceDroneBehaviour[] _defenceDrones;
    [SerializeField] private PlayerData _playerData;

    [SerializeField] private int _healAmount;
    [SerializeField] private int _nukesToAdd;


    public override void InstallBindings()
    {
        var targets = new Type[]
        {
            typeof(PlayerBonusHandler),
            typeof(PlayerAttacker),
            typeof(PlayerAttackerMultiple),
            typeof(PlayerHealthHandler),
            typeof(PlayerMover)
        };
        Container.BindInterfacesAndSelfTo<PlayerData>().FromInstance(_playerData).AsSingle().WhenInjectedInto(targets);

        Container.Bind<Timer>().WithId("BonusTimer").FromInstance(new Timer(_player)).AsCached().NonLazy();

        //Player
        Container.BindInterfacesAndSelfTo<RepairBonus>().AsTransient().WithArguments(_healAmount);
        Container.BindInterfacesAndSelfTo<MultilaserBonus>().AsTransient().WithArguments(_playerData.BonusLenght);
        Container.BindInterfacesAndSelfTo<ForceFieldBonus>().AsTransient().WithArguments(_playerData.BonusLenght);
        Container.BindInterfacesAndSelfTo<NukeBonus>().AsTransient().WithArguments(_nukesToAdd);
        Container.BindInterfacesAndSelfTo<DroneBonus>().AsTransient();

        Container.BindInterfacesAndSelfTo<PlayerBonusHandler>().FromNew().AsSingle().WithArguments(_player, _defenceDrones);
        Container.BindInterfacesAndSelfTo<PlayerHealthHandler>().AsCached().WithArguments(_player.gameObject);
        Container.Bind<Player>().FromInstance(_player).AsSingle().NonLazy();

        //PlayerController
        Container.BindInterfacesAndSelfTo<PlayerAttacker>().WithArguments(_player.transform).WhenInjectedInto<PlayerController>();
        Container.BindInterfacesAndSelfTo<PlayerAttackerMultiple>().WithArguments(_player.transform).WhenInjectedInto<PlayerController>();
        Container.BindInterfacesAndSelfTo<PlayerMover>().WithArguments(_player.transform).WhenInjectedInto<PlayerController>();
        Container.Bind<float>().FromInstance(_playerData.Speed).WhenInjectedInto<PlayerController>();
    }
}
