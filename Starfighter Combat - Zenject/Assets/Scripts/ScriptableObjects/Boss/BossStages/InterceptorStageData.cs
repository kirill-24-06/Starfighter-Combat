using UnityEngine;

[CreateAssetMenu(fileName = "InterceptorStage", menuName = "Config Data/Spawnable Data/Boss/BossStages/InterceptorStage")]
public class InterceptorStageData : BossStageData, IShooterData
{
    [SerializeField] protected float _reloadTime = 1.0f;
    [SerializeField] protected AudioClip _fireSound;
    [SerializeField, Range(0.1f, 1)] protected float _fireSoundVolume;

    [SerializeField] protected int _shotsBeforePositionChange;
    [SerializeField] protected float _stageSwitchHealthPercent = 50.0f;

    public float StageSwitchHealthPercent => _stageSwitchHealthPercent;
    public int ShotsBeforePositionChange => _shotsBeforePositionChange;

    public float ReloadCountDown => _reloadTime;

    public AudioClip FireSound => _fireSound;

    public float FireSoundVolume => _fireSoundVolume;

    public override BossStage GetBossStage() => new InterceptorStage(this);
}