using UnityEngine;

[CreateAssetMenu(fileName = "New ProjectileData", menuName = "Config Data/Projectile Data", order = 54)]
public class ProjectileData : ScriptableObject
{
    [SerializeField] protected ObjectTag _objectTag = ObjectTag.None;

    [SerializeField] protected float _objectSpeed;

    [SerializeField] protected Vector2 _gameZoneBorders;

    [SerializeField] protected int _damage;

    public ObjectTag Tag => _objectTag;
    public float Speed => _objectSpeed;
    public Vector2 GameZoneBorders => _gameZoneBorders;

    public int Damage => _damage;
}