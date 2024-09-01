using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StageData", menuName = "Config Data/Level/Level Data", order = 53)]
public class LevelData : ScriptableObject
{
    [SerializeField] private List<StageData> _stages;

    public List<StageData> Stages => _stages;
}
