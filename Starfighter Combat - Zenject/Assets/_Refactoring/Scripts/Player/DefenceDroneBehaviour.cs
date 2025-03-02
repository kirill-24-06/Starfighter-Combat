using UnityEngine;
using Utils.SpawnSystem;

namespace Refactoring
{
    public class DefenceDroneBehaviour : MonoBehaviour
    {
        private GameObject _gameObject;
        private Transform _transform;

        private IFactory<MonoProduct> _explosionEffectFactory;

        private IWeapon _weapon;
        private IResetable _weaponReset;

        [Zenject.Inject]
        public void Construct(
            DefenceDroneData data,
            [Zenject.Inject(Id = "ExplosionEffect")]IFactory<MonoProduct> explosionEffectFactory,
            [Zenject.Inject(Id = "PlayerMissile")] IFactory<MonoProduct> projectileFactory)
        {
            _transform = transform;
            _gameObject = gameObject;

            _explosionEffectFactory = explosionEffectFactory;

            var weapon = new EnemyCannon(this, projectileFactory, data);
            _weapon = weapon;
            _weaponReset = weapon;

            gameObject.SetActive(false);
        }

        private void OnEnable() => _weaponReset?.Reset();
        private void Update() => _weapon.Attack();

        public void Disable()
        {
            var explosion = _explosionEffectFactory.Create();
            explosion.transform.SetLocalPositionAndRotation(_transform.position, explosion.transform.rotation);

            _gameObject.SetActive(false);
        }
    }

}