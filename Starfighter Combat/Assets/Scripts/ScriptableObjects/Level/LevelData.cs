using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StageData", menuName = "Config Data/Level/Level Data", order = 53)]
public class LevelData : ScriptableObject
{
    [SerializeField] private List<StageData> _stages;
    [SerializeField] private BossWave _bossWave;
    [SerializeField] private float _bossWaveDelay;

    public List<StageData> Stages => _stages;

    public BossWave BossWave => _bossWave;

    public int BossWaveDelay => (int)(_bossWaveDelay * GlobalConstants.MillisecondsConverter);
}
