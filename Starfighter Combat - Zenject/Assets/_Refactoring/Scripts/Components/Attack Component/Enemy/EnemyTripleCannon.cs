using UnityEngine;
using System;
using System.Collections.Generic;
using Utils.SpawnSystem;

namespace Refactoring
{
    public class EnemyTripleCannon : IWeapon, IResetable
    {
        protected IAdvancedWeaponData _data;

        private GameObject _client;
        private List<Transform> _firePoints;

        private IFactory<MonoProduct> _projectileFactory;

        private AudioSource _audioPlayer;

        protected readonly Timer _reloadTimer;

        protected bool _onReload = false;

        private int _shotsFired = 0;

        public event Action AttackRunComplete;

        public EnemyTripleCannon(MonoBehaviour client, IFactory<MonoProduct> projectileFactory, IAdvancedWeaponData data)
        {
            _data = data;

            _client = client.gameObject;

            _firePoints = new List<Transform>();
            FindFirePoints(_client.transform);

            _projectileFactory = projectileFactory;

            _audioPlayer = _client.GetComponentInChildren<AudioSource>();

            _reloadTimer = new Timer(client);
            _reloadTimer.TimeIsOver += OnReloadTimerExpired;
        }

        public void Attack()
        {
            if (_onReload) return;

            _onReload = true;

            for (int i = 0; i < _firePoints.Count; i++)
            {
                var projectile = _projectileFactory.Create();
                projectile.transform.SetLocalPositionAndRotation(_firePoints[i].position, _firePoints[i].rotation);
            }

            _shotsFired++;

            if (_shotsFired >= _data.AttacksBeforeReposition)
            {
                AttackRunComplete?.Invoke();
                _shotsFired = 0;
            }

            _audioPlayer.PlayOneShot(_data.FireSound, _data.FireSoundVolume);
            Reload();
        }

        private void Reload()
        {
            if (!_client.activeInHierarchy) return;

            _reloadTimer.SetTimer(_data.ReloadCountDown);
            _reloadTimer.StartTimer();
        }

        public void Reset()
        {
            _reloadTimer.StopTimer();
            _onReload = false;
            _shotsFired = 0;
        }

        protected void OnReloadTimerExpired() => _onReload = false;

        private void FindFirePoints(Transform client)
        {
            foreach (Transform child in client)
            {
                if (child.name == "FirePoint")
                    _firePoints.Add(child);
            }
        }
    }
}
