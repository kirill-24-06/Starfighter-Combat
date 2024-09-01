using UnityEngine;

public enum BonusTag
{
    Health,
    Multilaser,
    LaserBeam,
    ForceField,
    IonSphere,
    DefenceDrone,
    None,
}

[CreateAssetMenu(fileName = "New ObjectData", menuName = "Config Data/Object Data", order = 51)]
public class ObjectsData : ScriptableObject
{
    [SerializeField] protected ObjectTag _objectTag = ObjectTag.None;
    [SerializeField] private BonusTag _bonusTag = BonusTag.None;

    [SerializeField] protected int _health;
    [SerializeField] protected float _objectSpeed;

    [SerializeField] protected int _score;

    [SerializeField] protected GameObject _weaponProjectile;
    [SerializeField] protected float _reloadTime;

    [SerializeField] protected Vector2 _gameZoneBorders;

    public ObjectTag Tag => _objectTag;

    public BonusTag BonusTag => _bonusTag;

    public int Health => _health;

    public float Speed => _objectSpeed;

    public GameObject Projectile => _weaponProjectile;

    public Vector2 GameZoneBorders => _gameZoneBorders;
    
    public float ReloadTime => _reloadTime;

    public int Score => _score;
}