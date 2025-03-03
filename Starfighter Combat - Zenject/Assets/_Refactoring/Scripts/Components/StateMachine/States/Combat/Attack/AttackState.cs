using UnityEngine;

namespace Refactoring
{
    public class AttackState : EnemyCombatState
    {
        private IWeapon _weapon;

        private Transform _target;

        public bool AttackComplete { get; private set; }

        public AttackState(Transform client, IWeapon weapon, Transform target) : base(client)
        {
            _weapon = weapon;
            _target = target;
        }

        public override void FixedUpdate() => Attack();
        public override void Exit() => AttackComplete = false;


        private void Attack()
        {
            if (_target.gameObject.activeInHierarchy)
            {
                RotateTowardsTarget(_target.position);
                _weapon.Attack();
            }
        }

        public void OnAttackComplete() => AttackComplete = true;
    }

}
