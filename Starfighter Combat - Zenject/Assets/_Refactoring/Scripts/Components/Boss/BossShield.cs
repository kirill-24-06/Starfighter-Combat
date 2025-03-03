using UnityEngine;
using Utils.Events.Channel.Static;

namespace Refactoring
{
    public class BossShield : MonoBehaviour, IInteractableEnemy
    {
        [SerializeField] private PolygonCollider2D _bossCollider;

        [SerializeField] private int _maxHealth = 15;
        private int _currentHealth;

        private CollisionMap _collisionMap;

        private Player _player;

        public bool IsActive { get; private set; } = false;

        public void Initialize(Player player, CollisionMap collisionMap)
        {
            _collisionMap = collisionMap;
            _collisionMap.Register(GetComponent<Collider2D>(), this);

            _player = player;
        }

        private void OnEnable()
        {
            _currentHealth = _maxHealth;
            IsActive = true;
            _bossCollider.enabled = false;
        }

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            if (Player.IsPlayer(collision.gameObject) || collision.gameObject == _player.ForceField.gameObject)
                Collide();
        }
        private void Collide()
        {
            if(!_player.IsInvunerable && !_player.IsDroneActive)
            {
                Channel<PlayerDamagedEvent>.Raise(new PlayerDamagedEvent(GlobalConstants.CollisionDamage));
            }

            else if(!_player.IsInvunerable && _player.IsDroneActive)
            {
                Channel<DroneDestroyedEvent>.Raise(new DroneDestroyedEvent());
            }

            TakeDamage(GlobalConstants.CollisionDamage);
        }

        public void Interact() => TakeDamage(GlobalConstants.CollisionDamage);

        public void TakeDamage(int damage)
        {

            _currentHealth -= damage;

            if (_currentHealth < 0)
                _currentHealth = 0;

            if (_currentHealth == 0)
                gameObject.SetActive(false);
        }

        public void RenewShield()
        {
            _currentHealth = _maxHealth;
        }

        private void OnDisable()
        {
            IsActive = false;
            _bossCollider.enabled = true;
        }
    }
}