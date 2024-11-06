using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class PlayerBonusHandler : IBonusHandler, IResetable
{
    private Player _player;
    private PlayerData _playerData;
    private EventManager _events;
    private Timer _bonusTimer;

    private DefenceDroneBehaviour[] _defenceDrones;

    private GameObject _nukePrefab;
    private Transform _nukePoint;
    private Collider2D[] _nukeTargets;

    private int _nukesAmount = 0;
    private int _dronesAmount = 0;

    private bool _isEquiped;
    private bool _isDroneActive;
    private bool _bonusIsActive = false;

    private CancellationToken _sceneExitToken;

    public bool IsDroneActive => _isDroneActive;
    public bool BonusIsActive => _bonusIsActive;
    public bool IsEquiped => _isEquiped;

    public Timer BonusTimer => _bonusTimer;

    public PlayerBonusHandler(Player player)
    {
        _player = player;
        _playerData = _player.PlayerData;
        _bonusTimer = new Timer(_player);

        _defenceDrones = _player.GetComponentsInChildren<DefenceDroneBehaviour>();

        _nukePrefab = _playerData.NukePrefab;
        _nukePoint = _player.transform.Find("NukePoint");
        _nukeTargets = new Collider2D[35];

        _events = EntryPoint.Instance.Events;

        _sceneExitToken = EntryPoint.Instance.destroyCancellationToken;

        _events.BonusCollected += Handle;
        _events.MultilaserEnd += OnMultilaserEnd;
        _events.DroneDestroyed += OnDroneDestruction;
        _events.IonSphereUse += OnNukeUse;
    }

    public void OnStart()
    {
        _nukesAmount = _playerData.NukesStartAmount;
        _isEquiped = _nukesAmount > 0;
        _events.BonusAmountUpdate?.Invoke(_nukesAmount);
    }

    public void Handle(BonusTag tag)
    {
        switch (tag)
        {
            case BonusTag.Health:

                _events.PlayerHealed?.Invoke(1);
                break;

            case BonusTag.Multilaser:

                EnableMultilaser();
                break;

            case BonusTag.ForceField:

                ActivateForceField();
                break;

            case BonusTag.IonSphere:

                AddNuke();
                break;

            case BonusTag.DefenceDrone:

                ActivateDrone();
                break;
        }
    }

    private void EnableMultilaser()
    {
        _bonusIsActive = true;

        _events.Multilaser?.Invoke(true);

        _bonusTimer.SetTimer(_playerData.BonusTimeLenght);
        _bonusTimer.TimeIsOver += _events.MultilaserEnd;
        _bonusTimer.StartTimer();

        EntryPoint.Instance.HudManager.ActivateBonusTimer();
    }

    private void OnMultilaserEnd()
    {
        _bonusIsActive = false;
        _events.Multilaser?.Invoke(false);

        _bonusTimer.ResetTimer();
    }

    public void ActivateForceField()
    {
        _events.ForceField?.Invoke();

        _bonusTimer.SetTimer(_playerData.BonusTimeLenght);
        _bonusTimer.TimeIsOver += _events.ForceFieldEnd;
        _bonusTimer.StartTimer();

        EntryPoint.Instance.HudManager.ActivateBonusTimer();
    }

    private void AddNuke(int amount = 1)
    {
        _nukesAmount += amount;
        _isEquiped = _nukesAmount > 0;
        EntryPoint.Instance.Events.BonusAmountUpdate?.Invoke(_nukesAmount);
    }

    private void OnNukeUse() => UseNuke().Forget();

    private async UniTaskVoid UseNuke()
    {
        _player.StartTempInvunrability().Forget();

        ObjectPool.Get(_nukePrefab, _nukePoint.position,
              _nukePrefab.transform.rotation);

        var count = Physics2D.OverlapCircleNonAlloc(_nukePoint.position, 30f, _nukeTargets);

        await UniTask.Delay(300, cancellationToken: _sceneExitToken);

        for (int i = 0; i < count; i++)
        {
            if (EntryPoint.Instance.CollisionMap.NukeInteractables.TryGetValue(_nukeTargets[i], out INukeInteractable interactable))
            {
                interactable.GetDamagedByNuke();
            }
        }

        _nukesAmount--;
        _isEquiped = _nukesAmount > 0;

        _events.BonusAmountUpdate?.Invoke(_nukesAmount);
    }

    private void ActivateDrone()
    {
        if (_dronesAmount < _defenceDrones.Length)
        {
            _dronesAmount++;
            _isDroneActive = _dronesAmount > 0;

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
        _isDroneActive = _dronesAmount > 0;

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

    public void Reset()
    {
        _events.BonusCollected -= Handle;
        _events.MultilaserEnd -= OnMultilaserEnd;
        _events.DroneDestroyed -= OnDroneDestruction;
        _events.IonSphereUse -= OnNukeUse;
    }
}