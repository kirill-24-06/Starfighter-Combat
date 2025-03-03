using UnityEngine;
using Utils.SpawnSystem;

namespace Refactoring
{
    public class EnemyCannon : IWeapon, IResetable
    {
        private IWeaponData _data;

        private GameObject _client;
        private Transform _firePoint;

        private IFactory<MonoProduct> _projectileFactory;

        private AudioSource _audioPlayer;

        protected readonly Timer _reloadTimer;

        protected bool _isOnReload = false;

        public EnemyCannon(MonoBehaviour client,IFactory<MonoProduct> projectileFactory, IWeaponData shooterData)
        {
            _data = shooterData;

            _client = client.gameObject;
            _firePoint = _client.transform.Find("FirePoint");

            _projectileFactory = projectileFactory;

            _audioPlayer = _client.GetComponentInChildren<AudioSource>();
            
            _reloadTimer = new Timer(client);
            _reloadTimer.TimeIsOver += OnReloadTimerExpired;
        }

        public void Attack()
        {
            if (_isOnReload) return;

            _isOnReload = true;

            var projectile = _projectileFactory.Create();
            projectile.transform.SetLocalPositionAndRotation(_firePoint.position, _firePoint.rotation);

            if (!_client.activeInHierarchy) return;

            _audioPlayer.PlayOneShot(_data.FireSound, _data.FireSoundVolume);

            Reload();
        }

        private void Reload()
        {
            _reloadTimer.SetTimer(_data.ReloadCountDown);
            _reloadTimer.StartTimer();
        }

        public void Reset()
        {
            _reloadTimer.StopTimer();
            _isOnReload = false;
        }

        protected void OnReloadTimerExpired() => _isOnReload = false;
    }

}
