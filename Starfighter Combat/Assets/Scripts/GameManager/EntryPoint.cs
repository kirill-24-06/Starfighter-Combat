using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private BackgroundMover _backgroundMover;
   

    //since the EntryPoint object exists throughout the game,
    //but is called only if the scene is initialized,
    //it will also be responsible for class without Monobahaviour coroutines
    public static EntryPoint Bootstrap { get; private set; }

    private void Awake()
    {
        Initialize();

        _backgroundMover.Initialise();
        _playerController.Initialise();
    }

    private void Initialize()
    {
        if (Bootstrap == null)
        {
            Bootstrap = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
