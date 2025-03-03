using UnityEngine;
using Utils.SpawnSystem;

namespace Refactoring
{
    public abstract class Projectile : MonoProduct, INukeInteractable
    {
     
        protected IProjectileBaseData _projectileBaseData;

        protected IFactory<MonoProduct> _collisionEffectFactory;
        protected IFactory<MonoProduct> _explosionEffectFactory;

        protected GameObject _gameObject;
        protected Transform _transform;
        protected Collider2D _collider;

        protected CollisionMap _collisionMap;

        protected bool _isPooled = true;

        protected abstract void Collide(Collision2D collision);

        protected void Awake()
        {
            _gameObject = gameObject;
            _transform = transform;
            _collider = GetComponent<Collider2D>();
        }

        private void Start()
        {
            _collisionMap.RegisterNukeInteractable(_collider, this);
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_isPooled) return;

            _isPooled = true;

            Release();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_isPooled) return;
            _isPooled = true;

            Collide(collision);

            CreateVisualEffect(_collisionEffectFactory);

            Release();
        }

        public override MonoProduct OnGet()
        {
            if (!IsConstructed) return this;

            _isPooled = false;

            return this;
        }

        public void GetDamagedByNuke()
        {
            if (_isPooled) return;
            _isPooled = true;

            CreateVisualEffect(_explosionEffectFactory);

            Release();
        }

        protected void CreateVisualEffect(IFactory<MonoProduct> factory)
        {
            var effect = factory.Create();

            effect.gameObject.transform.SetLocalPositionAndRotation(transform.position, _projectileBaseData.ExplosionPrefab.transform.rotation);
        }
    }
}