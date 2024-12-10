using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StageData", menuName = "Config Data/Level/Level Data", order = 53)]
public class LevelData : ScriptableObject
{
    [SerializeField] private List<StageData> _stages;
    [SerializeField] private BossWave _bossWave;
    [SerializeField] private float _bossWaveDelay;

    [SerializeField] private List<PrewarmableData> _prewarmables;

    public List<StageData> Stages => _stages;

    public List<PrewarmableData>Prewarmables => _prewarmables;

    public BossWave BossWave => _bossWave;

    public int BossWaveDelay => (int)(_bossWaveDelay * GlobalConstants.MillisecondsConverter);
}
