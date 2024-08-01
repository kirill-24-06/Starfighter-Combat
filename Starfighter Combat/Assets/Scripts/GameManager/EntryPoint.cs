using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private BackgroundMover _backgroundMover;

    private void Awake()
    {
        _backgroundMover.Initialise();
        _playerController.Initialise();
    }
}
