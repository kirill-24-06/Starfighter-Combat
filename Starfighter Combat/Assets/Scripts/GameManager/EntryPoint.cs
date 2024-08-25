using Ui.DialogWindows;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    [SerializeField] private ScoreController _scoreController;
    [SerializeField] private Player _player;
    [SerializeField] private BackgroundMover _backgroundMover;
    [SerializeField] private BasicSpawnManager _spawnManager;
    [SerializeField] private UiRoot _root;
    [SerializeField] private HUDManager _hudManager;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private BombsBar _bombsBar;
    [SerializeField] private SpriteRenderer[] _patrolArea;

    private DialogManager _dialogManager;

    public static EntryPoint Instance { get; private set; }

    public Player Player => _player;

    public GameController GameController => _gameController;

    public ScoreController ScoreController => _scoreController;

    public HUDManager HUD => _hudManager;

    public DialogManager DialogManager => _dialogManager;

    public Bounds[] PatrolArea { get; private set; }

    public Transform UiRoot => _root.transform;



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
        _dialogManager = new DialogManager();

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
