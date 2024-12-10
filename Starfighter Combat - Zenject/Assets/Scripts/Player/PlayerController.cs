using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private Player _player;

    private EventManager _events;

    private InputAction _move;
    private InputAction _shoot;
    private InputAction _pause;
    private InputAction _useBonus;

    private IMover _mover;
    private Vector2 Direction => _move.ReadValue<Vector2>();

    private IAttacker _attackHandler;
    private PlayerAttacker _singleCannon;
    private PlayerAttackerMultiple _multipleCannons;


    private bool _isGameActive;
    private bool _isPaused;
    private bool _onCooldown;

    public void Initialise()
    {
        _player = EntryPoint.Instance.Player;
        _events = EntryPoint.Instance.Events;

        _mover = new Mover(transform);

        _singleCannon = new PlayerAttacker(_player);
        _multipleCannons = new PlayerAttackerMultiple(_player);
        _attackHandler = _singleCannon;

        _isPaused = false;

        _events.Stop += OnStop;
        _events.Pause += OnPause;
        _events.Multilaser += EnableMultilaser;

        var playerInput = GetComponent<PlayerInput>();

        _move = playerInput.actions["Move"];
        _shoot = playerInput.actions["Shoot"];
        _pause = playerInput.actions["Pause"];
        _useBonus = playerInput.actions["Bonus"];

        _shoot.performed += OnShoot;
        _pause.performed += OnPauseInput;
        _useBonus.performed += OnBonusUse;
    }

    private void Start() => _isGameActive = true;

    private void OnStop() => _isGameActive = false;

    private void OnPause(bool value) => _isPaused = value;

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (!_isGameActive || _isPaused) return;

        _attackHandler.Fire();
    }

    private void OnPauseInput(InputAction.CallbackContext context)
    {
        if (!_isPaused)
            EntryPoint.Instance.GameController.PauseGame(true);
    }

    private void OnBonusUse(InputAction.CallbackContext context)
    {
        if (!_player.IsEquiped || _onCooldown) return;

        UseSpehre();
        StartCooldown().Forget();
    }

    private void Update()
    {
        if (!_isGameActive || _isPaused) return;

        Move();
    }

    private void Move()
    {
        _mover.Move(Direction, _player.PlayerData.Speed);

        CheckBorders();
    }

    private void EnableMultilaser(bool isEnabled)
    {
        if (isEnabled)
        {
            _singleCannon.Reset();
            _attackHandler = _multipleCannons;
        }

        else if (!isEnabled)
        {
            _multipleCannons.Reset();
            _attackHandler = _singleCannon;
        }
    }

    private void UseSpehre() => _events.IonSphereUse?.Invoke();

    private async UniTaskVoid StartCooldown()
    {
        _onCooldown = true;

        await UniTask.Delay(_player.PlayerData.NukeCooldown, cancellationToken: _player.destroyCancellationToken);

        _onCooldown = false;
    }

    private void CheckBorders()
    {
        if (transform.position.x < -_player.PlayerData.GameZoneBorders.x)
            transform.position = new Vector3(-_player.PlayerData.GameZoneBorders.x, transform.position.y, transform.position.z);

        if (transform.position.x > _player.PlayerData.GameZoneBorders.x)
            transform.position = new Vector3(_player.PlayerData.GameZoneBorders.x, transform.position.y, transform.position.z);

        if (transform.position.y < -_player.PlayerData.GameZoneBorders.y)
            transform.position = new Vector3(transform.position.x, -_player.PlayerData.GameZoneBorders.y, transform.position.z);

        if (transform.position.y > _player.PlayerData.GameZoneBorders.y)
            transform.position = new Vector3(transform.position.x, _player.PlayerData.GameZoneBorders.y, transform.position.z);
    }

    private void OnDisable()
    {
        _singleCannon.Reset();
        _multipleCannons.Reset();
        _attackHandler = _singleCannon;
    }

    private void OnDestroy()
    {
        _events.Stop -= OnStop;
        _events.Pause -= OnPause;
        _events.Multilaser -= EnableMultilaser;

        _shoot.performed -= OnShoot;
        _pause.performed -= OnPauseInput;
        _useBonus.performed -= OnBonusUse;
    }
}