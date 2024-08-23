using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    [SerializeField] private ScoreController _scoreController;
    [SerializeField] private Player _player;
    [SerializeField] private BackgroundMover _backgroundMover;
    [SerializeField] private BasicSpawnManager _spawnManager;
    [SerializeField] private HUDManager _hudManager;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private BombsBar _bombsBar;
    [SerializeField] private SpriteRenderer[] _patrolArea;

    public static EntryPoint Instance { get; private set; }

    public Player Player => _player;

    public GameController GameController => _gameController;

    public HUDManager HUD => _hudManager;

    public Bounds[] PatrolArea { get; private set; }



    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;

        }

        else
        {
            Destroy(gameObject);
        }

        Initialize();
    }

    private void Start()
    {
        _gameController.StartGame();
    }

    private void Initialize()
    {
        PatrolAreaInit();
        _player.Initialise();
        _hudManager.Initialise();
        _healthBar.Initialise();
        _bombsBar.Initialise();
        _gameController.Initialise();
        _scoreController.Initialise();
        _spawnManager.Initialise();
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
