using UnityEngine;

[CreateAssetMenu(fileName = "New BonusData", menuName = "Config Data/Spawnable Data/Bonus", order = 53)]
public class BonusData : SpawnableData
{
    [Header("Bonus")]
    [SerializeField] private BonusTag _bonusTag = BonusTag.None;
    [SerializeField] protected Vector2 _gameArea;

    [SerializeField] protected float _liveTime;
    [SerializeField] protected float _positionChangeTime;

    public Vector2 GameArea => _gameArea;
    public BonusTag BonusTag => _bonusTag;
    public float LiveTime => _liveTime;
    public float PositionChangeTime => _positionChangeTime;

}
