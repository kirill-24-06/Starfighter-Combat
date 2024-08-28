using UnityEngine;

[CreateAssetMenu(fileName = "New BonusData", menuName = "Config Data/Bonus Data", order = 53)]
public class BonusData : ScriptableObject
{
    [SerializeField] protected ObjectTag _objectTag = ObjectTag.None;
    [SerializeField] private BonusTag _bonusTag = BonusTag.None;

    [SerializeField] protected Vector2 _disableBorders;
    [SerializeField] protected Vector2 _gameArea;

    [SerializeField] protected float _objectSpeed;
    [SerializeField] protected int _score;

    [SerializeField] protected float _liveTime;
    [SerializeField] protected float _positionChangeTime;

    public ObjectTag Tag => _objectTag;
    public BonusTag BonusTag => _bonusTag;
   
    public float Speed => _objectSpeed;
    public int Score => _score;

    public Vector2 DisableBorders => _disableBorders;
    public Vector2 GameArea => _gameArea;

    public float LiveTime => _liveTime;
    public float PositionChangeTime => _positionChangeTime;
}
