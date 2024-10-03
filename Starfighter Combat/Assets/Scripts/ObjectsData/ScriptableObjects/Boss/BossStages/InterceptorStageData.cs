using UnityEngine;

[CreateAssetMenu(fileName = "InterceptorStage", menuName = "Config Data/Spawnable Data/Boss/BossStages/InterceptorStage")]
public class InterceptorStageData : BossStageData, IShooterData
{
    [SerializeField] private float _reloadTime = 1.0f;
    [SerializeField] private AudioClip _fireSound;
    [SerializeField, Range(0.1f, 1)] private float _fireSoundVolume;

    [SerializeField] private int _shotsBeforePositionChange;
    [SerializeField] private float _stageSwitchHealthPercent = 50.0f;

    public float StageSwitchHealthPercent => _stageSwitchHealthPercent;
    public int ShotsBeforePositionChange => _shotsBeforePositionChange;

    public float ReloadCountDown => _reloadTime;

    public AudioClip FireSound => _fireSound;

    public float FireSoundVolume => _fireSoundVolume;

    public override BossStage GetBossStage() => new InterceptorStage(this);
}