using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class PlayerBonusHandler : IDisposable
{
    public Dictionary<BonusTag,IBonus> _bonuses;

    private PolygonCollider2D _playerCollider;
    private ForceFieldBehaviour _forceField;
    private EventManager _events;

    private SpriteRenderer _spriteRenderer;
    private Color _tempInvunrabilityColor = new Color(1, 1, 1, 0.2f);

    private GameObject _nukePrefab;
    private Transform _nukePoint;
    private Collider2D[] _nukeTargets;
    private int _nukeCooldown;
    private int _nukesAmount = 0;

    private int _invunrabilityLenght;

    private CancellationTokenSource _cts;
    private CancellationToken _disposeToken;

    private DefenceDroneBehaviour[] _defenceDrones;
    private int _dronesAmount;

    public bool OnCooldown { get; private set; } = false;
    public bool IsInvunerable { get; private set; } = false;
    public bool IsDroneActive { get; private set; } = false;
    public bool IsEquiped { get; private set; } = false;
    public float BonusLenght { get; private set; }

    public PlayerBonusHandler(Player player, DefenceDroneBehaviour[] drones,
       List<IBonus> bonuses, IBonusHandlerData data, EventManager events)
    {
        _bonuses = new Dictionary<BonusTag, IBonus>(bonuses.Count);
        for (int i = 0; i < bonuses.Count; i++)
            _bonuses.Add((BonusTag)i, bonuses[i]);
       
        _events = events;

        _playerCollider = player.gameObject.GetComponent<PolygonCollider2D>();

        _nukePrefab = data.NukePrefab;
        _nukesAmount = data.NukesStartAmount;
        _nukePoint = player.transform.Find("NukePoint");
        _nukeTargets = new Collider2D[55];
        _nukeCooldown = data.NukeCooldown;

        _forceField = player.ForceField;
        BonusLenght = data.BonusLenght;

        _defenceDrones = drones;

        _invunrabilityLenght = data.TempInvunrabilityTime;
        _spriteRenderer = player.transform.Find("Texture").GetComponent<SpriteRenderer>();

        _cts = new CancellationTokenSource();
        _disposeToken = _cts.Token;

        _events.Stop += OnStop;
        _events.NukesAdded += OnNukeAdded;
        _events.AddDrone += ActivateDrone;
        _events.Invunerable += OnForceFieldActive;
        _events.DroneDestroyed += OnDroneDestruction;
    }

    public void OnStart()
    {
        IsEquiped = _nukesAmount > 0;
        _events.BonusAmountUpdate?.Invoke(_nukesAmount);
    }

    private void OnStop() => IsInvunerable = true;

    public void Handle(BonusTag tag)
    {
        if (tag == BonusTag.TempInvunrability)
            StartTempInvunrability().Forget();
        else
            _bonuses[tag].Handle();
    }

    private void OnForceFieldActive(bool value)
    {
        IsInvunerable = value;
        _playerCollider.enabled = !IsInvunerable;
    }

    private void OnNukeAdded(int amount)
    {
        _nukesAmount += amount;
        IsEquiped = _nukesAmount > 0;

        _events.BonusAmountUpdate?.Invoke(_nukesAmount);
    }

    public void UseNuke()
    {
        UseNukeAsync().Forget();
        StartCooldown().Forget();
    }

    private async UniTaskVoid UseNukeAsync()
    {
        StartTempInvunrability().Forget();

        ObjectPool.Get(_nukePrefab, _nukePoint.position,
              _nukePrefab.transform.rotation);

        var count = Physics2D.OverlapCircleNonAlloc(_nukePoint.position, 30f, _nukeTargets);

        await UniTask.Delay(300, cancellationToken: _disposeToken);

        for (int i = 0; i < count; i++)
        {
            if (EntryPoint.Instance.CollisionMap.NukeInteractables.TryGetValue(_nukeTargets[i], out INukeInteractable interactable))
            {
                interactable.GetDamagedByNuke();
            }
        }

        _nukesAmount--;
        IsEquiped = _nukesAmount > 0;

        _events.BonusAmountUpdate?.Invoke(_nukesAmount);
    }

    private async UniTaskVoid StartCooldown()
    {
        OnCooldown = true;
        await UniTask.Delay(_nukeCooldown, cancellationToken: _disposeToken);
        OnCooldown = false;
    }

    private async UniTaskVoid StartTempInvunrability()
    {
        IsInvunerable = true;
        _spriteRenderer.color = _tempInvunrabilityColor;

        await UniTask.Delay(_invunrabilityLenght, cancellationToken: _disposeToken);

        _spriteRenderer.color = Color.white;

        if (!_forceField.ShieldActive)
            IsInvunerable = false;
    }

    private void ActivateDrone()
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
            _events.PlayerHealed?.Invoke(1);
        }
    }

    private void OnDroneDestruction()
    {
        _dronesAmount--;
        IsDroneActive = _dronesAmount > 0;

        for (int i = _defenceDrones.Length - 1; i >= 0; i--)
        {
            if (_defenceDrones[i].gameObject.activeInHierarchy)
            {
                ObjectPool.Get(_defenceDrones[i].Explosion,
                    _defenceDrones[i].transform.position,
                    _defenceDrones[i].Explosion.transform.rotation);

                _defenceDrones[i].gameObject.SetActive(false);
                break;
            }
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();

        _events.Stop -= OnStop;
        _events.NukesAdded -= OnNukeAdded;
        _events.Invunerable -= OnForceFieldActive;
        _events.DroneDestroyed -= OnDroneDestruction;
        _events.AddDrone -= ActivateDrone;
    }
}