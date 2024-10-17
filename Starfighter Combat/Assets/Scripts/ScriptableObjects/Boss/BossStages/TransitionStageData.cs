using UnityEngine;

[CreateAssetMenu(fileName = "new TransitionStage", menuName = "Config Data/Spawnable Data/Boss/BossStages/TransitionStage")]
public class TransitionStageData : BossStageData
{
    [SerializeField] private BossDefenceDroneData _defenceDrone;
    [SerializeField] private int _defenceDronesAmount;
    [SerializeField] private float _colorChangeDuration;

    public float ColorChangeDuration => _colorChangeDuration;
    public int DefenceDronesAmount => _defenceDronesAmount;

    public BossDefenceDroneData BossDefenceDrone => _defenceDrone;


    public override BossStage GetBossStage()
    {
        return new TransitionStage(this);
    }
}