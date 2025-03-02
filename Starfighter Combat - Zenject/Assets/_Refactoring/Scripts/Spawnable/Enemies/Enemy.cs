using Cysharp.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using Utils.SpawnSystem;

namespace Refactoring
{
    public enum EnemyStrenght
    {
        None,
        Basic,
        Hard
    }

    public abstract class Enemy : MonoProduct, INukeInteractable, IInteractableEnemy
    {
        protected IDamagebleData _enemyData;

        #region MonobechaviourData
        protected GameObject _gameObject;
        protected Transform _transform;
        protected Collider2D _collider;
        #endregion

        #region Events

        protected EnemyDestroyedEvent _enemyDestroyed;
        protected AddScoreEvent _addScore;
        protected PlayGlobalSoundEvent _explosionSound;

        protected PlayerDamagedEvent _damage;
        protected DroneDestroyedEvent _destroyDrone;

        #endregion

        protected IMoveComponent _moveHandler;
        protected IDamageble _damageHandler;
        protected List<IResetable> _resetables;

        protected CollisionMap _collisionMap;
        protected IFactory<MonoProduct> _explosionEffectsFactory;
        protected Player _player;

        protected CancellationToken _sceneExitToken;

        public bool IsInPool { get; protected set; } = true;

        protected abstract void OnDead();

        #region Monobechaviuor

        protected virtual void Awake()
        {
            _gameObject = gameObject;
            _transform = transform;
            _collider = GetComponent<Collider2D>();

            _damage = new PlayerDamagedEvent(GlobalConstants.CollisionDamage);
            _destroyDrone = new DroneDestroyedEvent();
        }

        protected virtual void Start()
        {
            _collisionMap.Register(_collider, this);
            _collisionMap.RegisterNukeInteractable(_collider, this);
            _collisionMap.RegisterMissileTarget(_transform);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (Player.IsPlayer(collision.gameObject) || collision.gameObject == _player.ForceField.GameObject)
                Collide();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsInPool) return;
            IsInPool = true;

            Utils.Events.Channel.Static.Channel<EnemyDestroyedEvent>.Raise(_enemyDestroyed);
            Release();
        }

        #endregion

        #region MonoProduct

        public override MonoProduct OnGet()
        {
            if (!IsConstructed) return this;

            foreach (var resetable in _resetables) resetable.Reset();

            IsInPool = false;

            return this;
        }

        #endregion

        protected virtual void Collide()
        {
            if (!_player.IsInvunerable && !_player.IsDroneActive)
                Utils.Events.Channel.Static.Channel<PlayerDamagedEvent>.Raise(_damage);

            else if (!_player.IsInvunerable && _player.IsDroneActive)
                Utils.Events.Channel.Static.Channel<DroneDestroyedEvent>.Raise(_destroyDrone);

            _damageHandler.TakeDamage(GlobalConstants.CollisionDamage * _enemyData.Health);
        }

        public virtual void Interact() => _damageHandler.TakeDamage(GlobalConstants.CollisionDamage);

        public virtual void GetDamagedByNuke() => GetDamagedByNukeAsync().Forget();

        private async UniTaskVoid GetDamagedByNukeAsync()
        {
            await UniTask.Delay(Random.Range(175, 355), cancellationToken: _sceneExitToken);
            _damageHandler.TakeDamage(GlobalConstants.NukeDamage);
        }

        protected void CreateExplosion()
        {
            var explosion = _explosionEffectsFactory.Create();
            explosion.gameObject.transform.SetLocalPositionAndRotation(transform.position, _enemyData.Explosion.transform.rotation);
        }
    }
}
