using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player _player;
    private AudioSource _playerAudio;

    private EventManager _events;
    private IInput _inputHandler;

    private IAttacker _attackHandler;
    private PlayerAttacker _singleCannon;
    private PlayerAttackerMultiple _multipleCannons;


    private bool _isGameActive;
    private bool _isPaused;
    private bool _onCooldown;

    public void Initialise()
    {
        _player = EntryPoint.Instance.Player;
        _playerAudio = GetComponentInChildren<AudioSource>();
        _events = EntryPoint.Instance.Events;

        _inputHandler = new KeyboardInput();

        _singleCannon = new PlayerAttacker(_player);
        _multipleCannons = new PlayerAttackerMultiple(_player);
        _attackHandler = _singleCannon;

        _isPaused = false;

        _events.Stop += OnStop;
        _events.Pause += OnPause;
        _events.Multilaser += EnableMultilaser;
    }

    private void Start() => _isGameActive = true;

    private void OnStop() => _isGameActive = false;

    private void OnPause(bool value) => _isPaused = value;

    private void Update()
    {
        if (_inputHandler.PauseInput() && !_isPaused)
        {
            EntryPoint.Instance.GameController.PauseGame(true);
        }

        if (!_isGameActive || _isPaused)
            return;

        Move(_inputHandler.MoveInput(), _player.PlayerData.Speed);
        CheckBorders();

        if (_inputHandler.ShootInput())
        {
            _attackHandler.Fire();
            _playerAudio.PlayOneShot(_player.PlayerData.FireSound, _player.PlayerData.FireSoundVolume);
        }

        if (_inputHandler.BonusInput() && _player.IsEquiped && !_onCooldown)
        {
            UseSpehre();
            StartCooldown().Forget();
        }
    }

    private void OnDisable()
    {
        _singleCannon.Reset();
        _multipleCannons.Reset();
        _attackHandler = _singleCannon;
    }

    public void Move(Vector3 direction, float speed)
    {
        direction = direction.normalized;
        _player.transform.Translate(speed * Time.deltaTime * direction);
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

    private void OnDestroy()
    {
        _events.Stop -= OnStop;
        _events.Pause -= OnPause;
        _events.Multilaser -= EnableMultilaser;
    }
}