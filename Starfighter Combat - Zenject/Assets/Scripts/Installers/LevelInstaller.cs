using UnityEngine;
using Zenject;

public class LevelInstaller: MonoInstaller
{
    [SerializeField] private LevelData _levelData;

    [SerializeField] private SpriteRenderer[] _patrolArea;
    [SerializeField] private Transform _gameObjectsHolder;
    [SerializeField] private SpawnArea[] _spawnAreas;

    [SerializeField] private Context _context;

    public override void InstallBindings()
    {
        Container.Bind<Spawner>().FromNew().AsCached().WithArguments(_spawnAreas);
        Container.Bind<CollisionMap>().FromNew().AsSingle().NonLazy();
        Container.Bind<PoolRootMap>().FromNew().AsSingle().WithArguments(_gameObjectsHolder).NonLazy();
        Container.Bind<SpawnController>().FromNew().AsSingle().WithArguments(_levelData.Stages.Count).NonLazy();

        var stages = new LevelStage[_levelData.Stages.Count];
        for (int i = 0; i < stages.Length; i++)
            stages[i] = new LevelStage(new Timer(_context), _levelData.Stages[i]);
       
        Container.BindInterfacesAndSelfTo<LevelController>().FromNew().AsSingle().WithArguments(_levelData,stages).NonLazy();

    }
}
