using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;


[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private PlayerBonusHandler _bonusHandler;
    private GameController _gameController;
    private EventManager _events;

    private InputAction _move;
    private InputAction _shoot;
    private InputAction _pause;
    private InputAction _useBonus;

    private IMover _mover;
    private float _speed;
    private Vector2 Direction => _move.ReadValue<Vector2>();

    private IAttacker _attackHandler;
    private List<IAttacker> _attackers;
    private List<IResetable> _resetables;

    private bool _isGameActive;
    private bool _isPaused;
    private bool _onCooldown;

    [Inject]
    public void Construct(List<IAttacker> attackers, List<IResetable> resetables, IMover mover,
        float speed, PlayerBonusHandler bonusHandler, GameController controller, EventManager events)
    {
        _bonusHandler = bonusHandler;
        _gameController = controller;
        _events = events;
        _mover = mover;
        _speed = speed;
        _attackers = attackers;
        _resetables = resetables;
        _attackHandler = _attackers[0];

        _isPaused = false;

        var playerInput = GetComponent<PlayerInput>();

        _move = playerInput.actions["Move"];
        _shoot = playerInput.actions["Shoot"];
        _pause = playerInput.actions["Pause"];
        _useBonus = playerInput.actions["Bonus"];

        _shoot.performed += OnShoot;
        _useBonus.performed += OnBonusUse;

        _events.Start += OnStart;
        _events.Stop += OnStop;
        _events.Pause += OnPause;
        _events.Multilaser += EnableMultilaser;

    }

    private void Start() => _isGameActive = true;

    private void OnStart()=> _pause.performed += OnPauseInput;

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
            _gameController.PauseGame(true);
    }

    private void OnBonusUse(InputAction.CallbackContext context)
    {
        if (!_bonusHandler.IsEquiped || _onCooldown) return;

        UseSpehre();
    }

    private void Update() => Move();

    private void Move()
    {
        if (!_isGameActive || _isPaused) return;
        _mover.Move(Direction, _speed);
    }

    private void EnableMultilaser(bool isEnabled)
    {
        if (isEnabled)
        {
            _resetables[0].Reset();
            _attackHandler = _attackers[1];
        }

        else if (!isEnabled)
        {
            _resetables[1].Reset();
            _attackHandler = _attackers[0];
        }
    }

    private void UseSpehre() => _bonusHandler.UseNuke();

    private void OnDisable()
    {
        foreach (var resetable in _resetables)
            resetable.Reset();

        _attackHandler = _attackers[0];
    }

    private void OnDestroy()
    {
        _events.Start -= OnStart;
        _events.Stop -= OnStop;
        _events.Pause -= OnPause;
        _events.Multilaser -= EnableMultilaser;

        _shoot.performed -= OnShoot;
        _pause.performed -= OnPauseInput;
        _useBonus.performed -= OnBonusUse;
    }
}