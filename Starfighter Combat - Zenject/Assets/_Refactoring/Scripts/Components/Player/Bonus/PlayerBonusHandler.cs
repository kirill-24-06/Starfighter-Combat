using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;
using Utils.SpawnSystem;

namespace Refactoring
{
    public class PlayerBonusHandler : IDisposable
    {
        private IBonusHandlerData _data;

        private Dictionary<BonusTag, IBonus> _bonuses;

        private PolygonCollider2D _playerCollider;
        private ForceFieldBehaviour _forceField;

        private SpriteRenderer _spriteRenderer;
        private Color _tempInvunrabilityColor = new(1, 1, 1, 0.2f);

        private CollisionMap _collisionMap;

        private IFactory<MonoProduct> _nukeFactory;
        private Transform _nukePoint;
        private Collider2D[] _nukeTargets;
        private int _nukesAmount = 0;

        private BombsAmountUpdateEvent _bombsAmountUpdateEvent;
        private PlayerHealedEvent _playerHealed;

        private CancellationTokenSource _cts;
        private CancellationToken _disposeToken;

        private DefenceDroneBehaviour[] _defenceDrones;
        private int _dronesAmount;

        public bool OnCooldown { get; private set; } = false;
        public bool IsInvunerable { get; private set; } = false;
        public bool IsDroneActive { get; private set; } = false;
        public bool IsEquiped { get; private set; } = false;
        public float BonusLenght { get; private set; }

        public PlayerBonusHandler(
            Player player,
            DefenceDroneBehaviour[] drones,
            List<IBonus> bonuses,
            IFactory<MonoProduct> nukeFactory,
            IBonusHandlerData data,
            CollisionMap collisionMap)
        {
            _data = data;

            _bonuses = new Dictionary<BonusTag, IBonus>(bonuses.Count);
            for (int i = 0; i < bonuses.Count; i++)
                _bonuses.Add((BonusTag)i, bonuses[i]);

            _playerCollider = player.gameObject.GetComponent<PolygonCollider2D>();

            _nukeFactory = nukeFactory;
            _nukesAmount = data.NukesStartAmount;
            _nukePoint = player.transform.Find("NukePoint");
            _nukeTargets = new Collider2D[55];

            _forceField = player.ForceField;
            BonusLenght = data.BonusLenght;

            _defenceDrones = drones;

            _spriteRenderer = player.transform.Find("Texture").GetComponent<SpriteRenderer>();

            _collisionMap = collisionMap;

            _cts = new CancellationTokenSource();
            _disposeToken = _cts.Token;

            _playerHealed = new PlayerHealedEvent().SetInt(1);

            Utils.Events.Channel.Static.Channel<StopEvent>.OnEvent += OnStop;
            Utils.Events.Channel.Static.Channel<BombsAddedEvent>.OnEvent += OnNukeAdded;
            Utils.Events.Channel.Static.Channel<DroneAddedEvent>.OnEvent += ActivateDrone;
            Utils.Events.Channel.Static.Channel<InvunrableEvent>.OnEvent += OnForceFieldActive;
            Utils.Events.Channel.Static.Channel<DroneDestroyedEvent>.OnEvent += OnDroneDestruction;
        }

        public void OnStart()
        {
            IsEquiped = _nukesAmount > 0;
            Utils.Events.Channel.Static.Channel<BombsAmountUpdateEvent>.Raise(_bombsAmountUpdateEvent.SetInt(_nukesAmount));
        }

        private void OnStop(StopEvent @event) => IsInvunerable = true;

        public void Handle(BonusTag tag)
        {
            if (tag == BonusTag.TempInvunrability)
                StartTempInvunrability().Forget();
            else
                _bonuses[tag].Handle();
        }

        private void OnForceFieldActive(InvunrableEvent @event)
        {
            IsInvunerable = @event.IsInvunrable;
            _playerCollider.enabled = !IsInvunerable;
        }

        private void OnNukeAdded(BombsAddedEvent @event)
        {
            _nukesAmount += @event.Amount;
            IsEquiped = _nukesAmount > 0;

            Utils.Events.Channel.Static.Channel<BombsAmountUpdateEvent>.Raise(_bombsAmountUpdateEvent.SetInt(_nukesAmount));
        }

        public void UseNuke()
        {
            UseNukeAsync().Forget();
            StartCooldown().Forget();
        }

        private async UniTaskVoid UseNukeAsync()
        {
            StartTempInvunrability().Forget();

            var nuke = _nukeFactory.Create();
            nuke.transform.SetLocalPositionAndRotation(_nukePoint.position, nuke.transform.rotation);

            var count = Physics2D.OverlapCircleNonAlloc(_nukePoint.position, 30f, _nukeTargets);

            await UniTask.Delay(300, cancellationToken: _disposeToken);

            for (int i = 0; i < count; i++)
            {
                if (_collisionMap.NukeInteractables.TryGetValue(_nukeTargets[i], out INukeInteractable interactable))
                {
                    interactable.GetDamagedByNuke();
                }
            }

            _nukesAmount--;
            IsEquiped = _nukesAmount > 0;

            Utils.Events.Channel.Static.Channel<BombsAmountUpdateEvent>.Raise(_bombsAmountUpdateEvent.SetInt(_nukesAmount));
        }

        private async UniTaskVoid StartCooldown()
        {
            OnCooldown = true;
            await UniTask.Delay(TimeSpan.FromSeconds(_data.NukeCooldown), cancellationToken: _disposeToken);
            OnCooldown = false;
        }

        private async UniTaskVoid StartTempInvunrability()
        {
            IsInvunerable = true;
            _spriteRenderer.color = _tempInvunrabilityColor;

            await UniTask.Delay(TimeSpan.FromSeconds(_data.TempInvunrabilityTime), cancellationToken: _disposeToken);

            _spriteRenderer.color = Color.white;

            if (!_forceField.ShieldActive)
                IsInvunerable = false;
        }

        private void ActivateDrone(DroneAddedEvent @event)
        {
            if (_dronesAmount < _defenceDrones.Length)
            {
                _dronesAmount++;
                IsDroneActive = _dronesAmount > 0;

                foreach (DefenceDroneBehaviour drone in _defenceDrones)
                {
                    if (!drone.gameObject.activeInHierarchy)
                    {
                        drone.gameObject.SetActive(true);
                        break;
                    }
                }
            }
            else
            {
                Utils.Events.Channel.Static.Channel<PlayerHealedEvent>.Raise(_playerHealed);
            }
        }

        private void OnDroneDestruction(DroneDestroyedEvent @event)
        {
            _dronesAmount--;
            IsDroneActive = _dronesAmount > 0;

            for (int i = _defenceDrones.Length - 1; i >= 0; i--)
            {
                if (_defenceDrones[i].gameObject.activeInHierarchy)
                {
                    _defenceDrones[i].Disable();
                    break;
                }
            }
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();

            Utils.Events.Channel.Static.Channel<StopEvent>.OnEvent -= OnStop;
            Utils.Events.Channel.Static.Channel<BombsAddedEvent>.OnEvent -= OnNukeAdded;
            Utils.Events.Channel.Static.Channel<DroneAddedEvent>.OnEvent -= ActivateDrone;
            Utils.Events.Channel.Static.Channel<InvunrableEvent>.OnEvent -= OnForceFieldActive;
            Utils.Events.Channel.Static.Channel<DroneDestroyedEvent>.OnEvent -= OnDroneDestruction;
        }
    }

}