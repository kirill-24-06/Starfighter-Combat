using UnityEngine;

public class InterceptorStage : BossStage
{
    private InterceptorStageData _data;
    private Transform _target;
    private MovementControl _mover;
    private EnemyAdvancedAttacker _weapon;

    public InterceptorStage(InterceptorStageData stageData)
    {
        _data = stageData;
    }

    public override BossStage Initialise(Boss boss)
    {
        _boss = boss;
        _target = EntryPoint.Instance.Player.transform;
        _mover = new MovementControl(boss.transform, boss.Data);
        _weapon = new EnemyAdvancedAttacker(boss, _data, _data.ShotsBeforePositionChange);
        _weapon.AttackRunComplete += OnAttackRunComplete;
        return this;
    }
    public override bool CheckCompletion()
    {
        if (_boss.CurrentHealth * 100 / _boss.Data.MaxHealth <= _data.StageSwitchHealthPercent) return true;

        return false;
    }

    public override void Handle()
    {
        _mover.Move(_boss.Data.Speed);
        Attack();
    }

    private void Attack()
    {
        if (!_mover.IsMoving && _target.gameObject.activeInHierarchy)
        {
            _mover.LookInTargetDirection(_target.position);
            _weapon.Fire();
        }
    }

    private void OnAttackRunComplete()
    {
        _mover.SetNewDirection();
    }
}
