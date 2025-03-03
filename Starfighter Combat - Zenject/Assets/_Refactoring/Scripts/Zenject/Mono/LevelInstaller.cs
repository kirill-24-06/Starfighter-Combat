using UnityEngine;
using Zenject;

namespace Refactoring
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private LevelData _levelData;

        [SerializeField] private SpawnArea[] _spawnAreas;

        [SerializeField] private Context _context;

        [SerializeField] private LevelController _levelController;

        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<Spawner>()
                .FromNew()
                .AsCached()
                .WithArguments(_spawnAreas)
                .NonLazy();

            Container
                .BindInterfacesAndSelfTo<SpawnController>()
                .FromNew()
                .AsCached()
                .WithArguments(_levelData.Stages.Count)
                .NonLazy();

            var stages = new LevelStage[_levelData.Stages.Count];
            for (int i = 0; i < stages.Length; i++)
                stages[i] = new LevelStage(new Timer(_context), _levelData.Stages[i]);

            Container
                .BindInterfacesAndSelfTo<LevelController>()
                .FromNew()
                .AsCached()
                .WithArguments(_levelData, stages)
                .NonLazy();
        }
    }
}
