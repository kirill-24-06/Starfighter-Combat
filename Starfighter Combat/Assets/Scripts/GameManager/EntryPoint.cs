using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour _player;
    [SerializeField] private BackgroundMover _backgroundMover;
    [SerializeField] private BasicSpawnManager _spawnManager;
    [SerializeField] private UiManager _uiManager;
    [SerializeField] private SpriteRenderer[] _patrolArea;

    public static PlayerBehaviour Player { get; private set; }

    public static Bounds[] PatrolArea { get; private set; }


    private void Awake()
    {
        _player.Initialise();
        _uiManager.Initialise();
        _spawnManager.Initialise();
        _backgroundMover.Initialise();
        Initialize();
    }

    private void Start()
    {
        EventManager.GetInstance().Start?.Invoke();
    }

    private void Initialize()
    {
        Player = _player;

        PatrolArea = new Bounds[_patrolArea.Length];

        for (int i = 0; i < _patrolArea.Length; i++)
        {
            PatrolArea[i] = _patrolArea[i].bounds;
        }
    }
}
