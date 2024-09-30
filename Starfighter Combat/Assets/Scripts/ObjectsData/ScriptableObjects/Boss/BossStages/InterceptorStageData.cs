using UnityEngine;

[CreateAssetMenu(fileName ="InterceptorStage", menuName = "Config Data/Spawnable Data/Boss/BossStages/InterceptorStage")]
public class InterceptorStageData : BossStageData
{
    [SerializeField] private float _reloadTime = 1.0f;
    [SerializeField] private int _shotsBeforePositionChange;
    [SerializeField] private float _stageSwitchHealthPercent = 50.0f;

    public float StageSwitchHealthPercent => _stageSwitchHealthPercent;
    public int ShotsBeforePositionChange => _shotsBeforePositionChange;

    public float ReloadTime => _reloadTime;

    public override BossStage GetBossStage() => new InterceptorStage(this);
}