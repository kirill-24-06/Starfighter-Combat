using UnityEngine;

[CreateAssetMenu(fileName = "New ProjectileData", menuName = "Config Data/Projectile Data", order = 54)]
public class ProjectileData : PrewarmableData
{
    [SerializeField] protected float _objectSpeed;
    [SerializeField] protected GameObject _explosionPrefab;

    [SerializeField] protected Vector2 _gameZoneBorders;

    [SerializeField] protected int _damage;

    public GameObject ExplosionPrefab => _explosionPrefab;

    public float Speed => _objectSpeed;
    public Vector2 DisableBorders => _gameZoneBorders;

    public int Damage => _damage;
}