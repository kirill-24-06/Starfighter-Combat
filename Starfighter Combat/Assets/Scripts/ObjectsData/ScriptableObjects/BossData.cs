using UnityEngine;

public abstract class BossData : SpawnableData
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private BossStageData[] _stages;
    [SerializeField] private Vector2 _gameArea;
   
    public int MaxHealth => _maxHealth;
    public BossStageData[] Stages => _stages;
    public Vector2 GameArea => _gameArea;
}