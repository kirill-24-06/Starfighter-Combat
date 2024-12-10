using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public enum EnemyStrenght
{
    None,
    Basic,
    Hard
}

public abstract class Enemy : MonoBehaviour, IInteractableEnemy, INukeInteractable
{
    protected EventManager _events;
    protected AudioSource _soundPlayer;
    protected CancellationToken _sceneExitToken;
    protected GameObject _gameObject;
    protected Transform _transform;

    protected IDamageble _damageHandler;
    protected IResetable _health;

    protected abstract void Initialise();
    protected abstract void Move();
    protected abstract void OnDead();
    protected abstract void Collide();

    protected bool _isInPool = true;

    protected virtual void Awake()
    {
        _gameObject = gameObject;
        _transform = transform;
        _events = EntryPoint.Instance.Events;
        _soundPlayer = EntryPoint.Instance.GlobalSoundFX;
        _sceneExitToken = EntryPoint.Instance.destroyCancellationToken;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (Player.IsPlayer(collision.gameObject) || collision.gameObject == EntryPoint.Instance.Player.ForceField.gameObject)
            Collide();
    }

    public virtual void Interact() => _damageHandler.TakeDamage(GlobalConstants.CollisionDamage);

    public virtual void GetDamagedByNuke() => GetDamagedByNukeAsync().Forget();

    private async UniTaskVoid GetDamagedByNukeAsync()
    {
        await UniTask.Delay(Random.Range(175, 355), cancellationToken: _sceneExitToken);
        _damageHandler.TakeDamage(GlobalConstants.NukeDamage);
    }
}