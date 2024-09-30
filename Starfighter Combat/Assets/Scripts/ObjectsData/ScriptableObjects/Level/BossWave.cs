using UnityEngine;

[CreateAssetMenu(fileName = "New BossWave", menuName = "Config Data/Level/Boss Wave", order = 53)]
public class BossWave : StageData
{
    [SerializeField] private BossData[] _bosses;

    public BossData[] Bosses => _bosses;
}
