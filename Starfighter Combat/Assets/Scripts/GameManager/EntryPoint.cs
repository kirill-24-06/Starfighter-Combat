using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour _player;
    [SerializeField] private BackgroundMover _backgroundMover;
    [SerializeField] private BasicSpawnManager _spawnManager;

    public static PlayerBehaviour Player { get; private set; }


    private void Awake()
    {
        _player.Initialise();
        _spawnManager.Initialise();
        _backgroundMover.Initialise();
        Initialize();
    }

    private void Initialize()
    {
        Player = _player;
    }
}
