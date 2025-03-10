using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils.Events.Channel.Static;

namespace Refactoring
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerBonusHandler _bonusHandler;

        private InputAction _move;
        private InputAction _shoot;
        private InputAction _pause;
        private InputAction _useBonus;

        private IMover _mover;
        private float _speed;
        private Vector2 Direction => _move.ReadValue<Vector2>().normalized;

        private IWeapon _attackHandler;
        private IWeapon[] _attackers;
        private List<IResetable> _resetables;

        private bool _isGameActive;
        private bool _isPaused;

        [Zenject.Inject]
        public void Construct(IWeapon[] attackers, List<IResetable> resetables, IMover mover,
            float speed, PlayerBonusHandler bonusHandler)
        {
            _bonusHandler = bonusHandler;
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
        }

        private void Awake()
        {
            Channel<StartEvent>.OnEvent += OnStart;
            Channel<StopEvent>.OnEvent += OnStop;
            Channel<PauseEvent>.OnEvent += OnPause;
            Channel<MultilaserEvent>.OnEvent += EnableMultilaser;
        }

        private void Start() => _isGameActive = true;

        private void OnStart(StartEvent @event) => _pause.performed += OnPauseInput;

        private void OnStop(StopEvent @event) => _isGameActive = false;

        private void OnPause(PauseEvent @event) => _isPaused = @event.Value;

        private void OnShoot(InputAction.CallbackContext context)
        {
            if (!_isGameActive || _isPaused) return;

            _attackHandler.Attack();
        }

        private void OnPauseInput(InputAction.CallbackContext context)
        {
            if (!_isPaused)
            {
                Channel<PauseEvent>.Raise(new PauseEvent(true));
            }
        }

        private void OnBonusUse(InputAction.CallbackContext context)
        {
            if (!_bonusHandler.IsEquiped || _bonusHandler.OnCooldown) return;

            UseNuke();
        }

        private void Update() => Move();

        private void Move()
        {
            if (!_isGameActive || _isPaused) return;
            _mover.Move(Direction, _speed);
        }

        private void EnableMultilaser(MultilaserEvent @event)
        {
            if (@event.Enabled)
            {
                _resetables[0].Reset();
                _attackHandler = _attackers[1];
            }

            else if (!@event.Enabled)
            {
                _resetables[1].Reset();
                _attackHandler = _attackers[0];
            }
        }

        private void UseNuke() => _bonusHandler.OnNukeUse();

        private void OnDisable()
        {
            foreach (var resetable in _resetables)
                resetable.Reset();

            _attackHandler = _attackers[0];
        }

        private void OnDestroy()
        {
            Channel<StartEvent>.OnEvent -= OnStart;
            Channel<StopEvent>.OnEvent -= OnStop;
            Channel<PauseEvent>.OnEvent -= OnPause;
            Channel<MultilaserEvent>.OnEvent -= EnableMultilaser;

            _shoot.performed -= OnShoot;
            _pause.performed -= OnPauseInput;
            _useBonus.performed -= OnBonusUse;
        }
    }

}
