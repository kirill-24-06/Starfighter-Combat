using UnityEngine;
using Zenject;

public class SystemsInstaller : MonoInstaller
{
    [Header("Audio")]
    [SerializeField] private AudioSource _globalSoundFX;
    [SerializeField] private AudioSource _musicPlayer;
    [SerializeField] private AudioClips _audioClips;

    [Header("TimerContext")]
    [SerializeField] private Context _context;

    //ToDo
    [Header("ToDo")]
    [SerializeField] private BackgroundController _backgroundController;
    [SerializeField] private SpriteRenderer[] _patrolArea;


    public override void InstallBindings()
    {
        //other
        var patrolArea = new Bounds[_patrolArea.Length];
        for (int i = 0; i < patrolArea.Length; i++)
            patrolArea[i] = _patrolArea[i].bounds;
        Container.Bind<Bounds[]>().FromInstance(patrolArea).AsCached();

        Container.Bind<Timer>().FromNew().AsTransient().WithArguments(_context);

        //events
        Container.Bind<EventManager>().FromNew().AsSingle().NonLazy();

        //controllers
        Container.BindInterfacesAndSelfTo<GameController>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ScoreController>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ObjectPool>().AsSingle().NonLazy();

        //sound
        Container.Bind<AudioSource>().FromInstance(_globalSoundFX).AsSingle().NonLazy();
        Container.Bind<AudioSource>().FromInstance(_musicPlayer).WhenInjectedInto<MusicPlayer>();
        Container.Bind<AudioClips>().FromInstance(_audioClips).NonLazy();//move to project context
        Container.BindInterfacesAndSelfTo<MusicPlayer>().AsSingle().NonLazy();
    }
}
