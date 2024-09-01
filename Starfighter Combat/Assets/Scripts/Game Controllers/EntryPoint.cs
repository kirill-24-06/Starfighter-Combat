using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    [SerializeField] private ScoreController _scoreController;
    [SerializeField] private Player _player;
    [SerializeField] private BackgroundMover _backgroundMover;
    [SerializeField] private SpawnController _spawnController;
    [SerializeField] private LevelController _levelController;
    [SerializeField] private UiRoot _root;
    [SerializeField] private HUDManager _hudManager;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private BombsBar _bombsBar;
    [SerializeField] private SpriteRenderer[] _patrolArea;

    private EventManager _events;
    private ObjectHolder _spawnedObjects;
    private Spawner _spawner;

    public static EntryPoint Instance { get; private set; }

    public EventManager Events => _events;

    public Player Player => _player;

    public GameController GameController => _gameController;

    public ScoreController ScoreController => _scoreController;

    public ObjectHolder SpawnedObjects => _spawnedObjects;

    public SpawnController SpawnController => _spawnController;

    public LevelController LevelController => _levelController;

    public Spawner Spawner => _spawner;

    public Bounds[] PatrolArea { get; private set; }

    public Transform UiRoot => _root.transform;



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
        _gameController.StartGame();
    }

    private void Initialize()
    {
        _events = new EventManager();
        _spawnedObjects = new ObjectHolder();
        _spawner = new Spawner();

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
        _backgroundMover.Initialise();
    }

    private void PatrolAreaInit()
    {
        PatrolArea = new Bounds[_patrolArea.Length];

        for (int i = 0; i < _patrolArea.Length; i++)
        {
            PatrolArea[i] = _patrolArea[i].bounds;
        }
    }
}
