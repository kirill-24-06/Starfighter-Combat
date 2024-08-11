using UnityEngine;

[CreateAssetMenu(fileName = "New ObjectData", menuName = "Object Data", order = 51)]
public class ObjectsData : ScriptableObject
{
    [SerializeField] protected ObjectTag _objectTag = ObjectTag.None;

    [SerializeField] protected int _health;
    [SerializeField] protected float _objectSpeed;

    [SerializeField] protected int _score;

    [SerializeField] protected GameObject _weaponProjectile;
    [SerializeField] protected float _reloadTime;

    [SerializeField] protected Vector2 _gameZoneBorders;

    public ObjectTag Tag => _objectTag;

    public int Health => _health;

    public float Speed => _objectSpeed;

    public GameObject Projectile => _weaponProjectile;

    public Vector2 GameZoneBorders => _gameZoneBorders;
    
    public float ReloadTime => _reloadTime;

    public int Score => _score;
}