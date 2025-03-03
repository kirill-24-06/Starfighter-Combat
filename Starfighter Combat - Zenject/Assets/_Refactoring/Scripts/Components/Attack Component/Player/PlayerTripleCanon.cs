using System.Collections.Generic;
using UnityEngine;
using Utils.SpawnSystem;

namespace Refactoring
{
    public class PlayerTripleCanon : IWeapon, IResetable
    {
        private IWeaponData _data;

        private IFactory<MonoProduct> _projectileFactory;
        private readonly List<Transform> _firePoints;

        private AudioSource _playerAudio;

        private readonly Timer _reloadTimer;

        private bool _isShooted = false;

        public PlayerTripleCanon(Transform player,
            [Zenject.Inject(Id = "PlayerProjectile")] IFactory<MonoProduct> projectileFactory,
            Timer timer, IWeaponData data)
        {
            _data = data;

            _projectileFactory = projectileFactory;

            _firePoints = FindSpawnPoints(player);

            _playerAudio = player.GetComponentInChildren<AudioSource>();

            _reloadTimer = timer;
            _reloadTimer.TimeIsOver += OnReloadTimerExpired;
        }

        public void Attack()
        {
            if (!_isShooted)
            {
                _isShooted = true;

                for (int i = 0; i < _firePoints.Count; i++)
                {
                    var projectile = _projectileFactory.Create();
                    projectile.transform.
                        SetLocalPositionAndRotation(_firePoints[i].position, _firePoints[i].rotation);
                }

                _playerAudio.PlayOneShot(_data.FireSound, _data.FireSoundVolume);

                Reload();
            }
        }

        private void Reload()
        {
            _reloadTimer.SetTimer(_data.ReloadCountDown);
            _reloadTimer.StartTimer();
        }

        private List<Transform> FindSpawnPoints(Transform parrent)
        {
            var result = new List<Transform>();

            foreach (Transform child in parrent)
            {
                if (child.name == "ProjectilePosition")
                    result.Add(child);
            }
            return result;
        }

        public void Reset()
        {
            _reloadTimer.StopTimer();
            _isShooted = false;
        }

        private void OnReloadTimerExpired() => _isShooted = false;
    }

}
