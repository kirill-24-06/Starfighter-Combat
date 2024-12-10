using Ui.DialogWindows;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private AudioSource _globalSoundFX;
    [SerializeField] private MusicPlayer _musicPlayer;
    [SerializeField] private GameController _gameController;
    [SerializeField] private ScoreController _scoreController;
    [SerializeField] private Player _player;
    [SerializeField] private BackgroundController _backgroundController;
    [SerializeField] private SpawnController _spawnController;
    [SerializeField] private LevelController _levelController;
    [SerializeField] private UiRoot _root;
    [SerializeField] private HUDManager _hudManager;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private BombsBar _bombsBar;
    [SerializeField] private SpriteRenderer[] _patrolArea;

    private EventManager _events;
    private PoolMap _poolMap;
    private Spawner _spawner;
    private CollisionMap _collisionMap;

    public static EntryPoint Instance { get; private set; }

    public EventManager Events => _events;

    public Player Player => _player;

    public GameController GameController => _gameController;

    public ScoreController ScoreController => _scoreController;

    public SpawnController SpawnController => _spawnController;

    public Spawner Spawner => _spawner;

    public Bounds[] PatrolArea { get; private set; }

    public Transform UiRoot => _root.transform;

    public HUDManager HudManager => _hudManager;

    public AudioSource GlobalSoundFX => _globalSoundFX;

    public CollisionMap CollisionMap => _collisionMap;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        else
            Destroy(gameObject);

        Initialize();
    }

    private void Start()
    {
        _events.PrewarmRequired?.Invoke();
        DialogManager.ShowDialog<WelcomeDialog>();
    }

    private void Initialize()
    {
        _events = new EventManager();
        _spawner = new Spawner();
        _collisionMap = new CollisionMap();
        _poolMap = new PoolMap();


        PatrolAreaInit();
        _player.Initialise();
        _hudManager.Initialise();
        _healthBar.Initialise();
        _bombsBar.Initialise();
        _gameController.Initialise();
        _scoreController.Initialise();
        _spawner.Initialise();
        _spawnController.Initialise();
        _levelController.Initialise();
        _musicPlayer.Initialise();
        _backgroundController.Initialise();
        _poolMap.Initialise(_spawnController.transform);
    }

    private void PatrolAreaInit()
    {
        PatrolArea = new Bounds[_patrolArea.Length];

        for (int i = 0; i < _patrolArea.Length; i++)
            PatrolArea[i] = _patrolArea[i].bounds;
    }
   
}
