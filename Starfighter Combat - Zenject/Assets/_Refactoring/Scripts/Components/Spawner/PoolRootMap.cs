using UnityEngine;

namespace Refactoring
{
    public class PoolRootMap
    {
        private GameObject _objectPoolEmptyHolder;

        private static GameObject _ParticleSystemEmpty;
        private static GameObject _enemyEmpty;
        private static GameObject _weaponEmpty;
        private static GameObject _bonusEmpty;

        public PoolRootMap(Transform parrent)
        {
            _objectPoolEmptyHolder = new GameObject("Pool Map");
            _objectPoolEmptyHolder.transform.SetParent(parrent);

            _ParticleSystemEmpty = new GameObject("Particle Effects");
            _ParticleSystemEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);

            _enemyEmpty = new GameObject("Enemies");
            _enemyEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);

            _weaponEmpty = new GameObject("Weapons");
            _weaponEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);

            _bonusEmpty = new GameObject("Bonuses");
            _bonusEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
        }

        public static void SetParrentObject(GameObject gameObject, PoolType poolType)
        {
            GameObject parrent = poolType switch
            {
                PoolType.ParticleSystem => _ParticleSystemEmpty,
                PoolType.Enemy => _enemyEmpty,
                PoolType.Weapon => _weaponEmpty,
                PoolType.Bonus => _bonusEmpty,
                PoolType.None => null,
                _ => null,
            };

            gameObject.transform.SetParent(parrent.transform);
        }
    }

}
