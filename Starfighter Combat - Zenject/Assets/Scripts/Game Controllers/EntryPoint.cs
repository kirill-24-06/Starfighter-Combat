using Ui.DialogWindows;
using UnityEngine;
using Zenject;

public class EntryPoint : MonoBehaviour
{
    public static EntryPoint Instance { get; private set; }

    public EventManager Events { get; private set; }

    public Player Player { get; private set; }

    public GameController GameController { get; private set; }

    public SpawnController SpawnController { get; private set; }

    public Spawner Spawner { get; private set; }

    public Bounds[] PatrolArea { get; private set; }

    public Transform UiRoot { get; private set; }

    public AudioSource GlobalSoundFX { get; private set; }

    public CollisionMap CollisionMap { get; private set; }

    [Inject]
    public void Construct
        (EventManager events,
        GameController gameController,
        AudioSource globalSoundFX,
        Player player,
        Spawner spawner,
        SpawnController spawnController,
        CollisionMap collisionMap,
        Bounds[] patrolArea,
        Transform uiRoot)
    {

        if (Instance == null)
            Instance = this;

        else
            Destroy(gameObject);

        Events = events;
        GlobalSoundFX = globalSoundFX;
        GameController = gameController;
        Spawner = spawner;
        SpawnController = spawnController;
        CollisionMap = collisionMap;
        UiRoot = uiRoot;
        Player = player;
        PatrolArea = patrolArea;
    }

    private void Start()
    {
        Events.PrewarmRequired?.Invoke();
        DialogManager.ShowDialog<WelcomeDialog>();
    }
}
