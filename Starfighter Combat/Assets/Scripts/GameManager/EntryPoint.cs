using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour _player;
    [SerializeField] private BackgroundMover _backgroundMover;

    public static PlayerBehaviour Player { get; private set; }


    private void Awake()
    {
        Initialize();

        _player.Initialise();
        _backgroundMover.Initialise();
    }

    private void Initialize()
    {
        Player = _player;
    }
}
