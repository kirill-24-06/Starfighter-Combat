using System.Threading;
using UnityEngine;
using Zenject;

namespace Refactoring
{
    public class SystemsInstaller : MonoInstaller
    {
        [Header("Audio")]
        [SerializeField] private AudioSource _globalSoundFX;
        [SerializeField] private AudioSource _musicPlayer;
        [SerializeField] private AudioClips _audioClips;

        [Header("TimerContext")]
        [SerializeField] private Context _context;

        [Header("Patrol Areas")]
        [SerializeField] private SpriteRenderer[] _patrolArea;

        [Header("SpawnableGOHolder")]
        [SerializeField] private Transform _gameObjectsHolder;

        public override void InstallBindings()
        {
            //other
            var patrolArea = new Bounds[_patrolArea.Length];
            for (int i = 0; i < patrolArea.Length; i++)
            {
                patrolArea[i] = _patrolArea[i].bounds;
            }
            Container
                .Bind<Bounds[]>()
                .FromInstance(patrolArea)
                .AsCached();

            Container
                .Bind<CollisionMap>()
                .FromNew()
                .AsSingle()
                .NonLazy();

            var objectsHolder = new PoolRootMap(_gameObjectsHolder);
            Container
                .Bind<PoolRootMap>()
                .FromInstance(objectsHolder)
                .AsSingle()
                .WithArguments(_gameObjectsHolder)
                .NonLazy();

            Container
                .Bind<Timer>()
                .WithId("BonusTimer")
                .FromInstance(new Timer(_context))
                .AsCached()
                .NonLazy();

            //SceneExitToken
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            Container
                .Bind<CancellationToken>()
                .FromInstance(token)
                .AsSingle()
                .NonLazy();

            //controllers
            Container
                .BindInterfacesAndSelfTo<GameController>()
                .AsSingle()
                .WithArguments(cts)
                .NonLazy();

            Container
                .BindInterfacesAndSelfTo<ScoreController>()
                .AsSingle()
                .NonLazy();

            //sound
            var audio = new AudioPlayer(_musicPlayer, _globalSoundFX, _audioClips);
            Container.BindInterfacesAndSelfTo<AudioPlayer>().FromInstance(audio).AsSingle().NonLazy();

        }
    }
}
