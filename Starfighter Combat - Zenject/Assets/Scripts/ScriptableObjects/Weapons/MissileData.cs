using UnityEngine;

[CreateAssetMenu(fileName = "New ProjectileData", menuName = "Config Data/Missile Data", order = 55)]
public class MissileData : PrewarmableData
{
    //[SerializeField] protected ObjectTag _objectTag = ObjectTag.None;

    [SerializeField] protected float _objectSpeed;

    [SerializeField] protected Vector2 _gameZoneBorders;

    [SerializeField] protected int _damage;

    [SerializeField] private float _launchTime;

    [SerializeField] private float _homingTime;

    //public ObjectTag Tag => _objectTag;
    public float Speed => _objectSpeed;
    public Vector2 DisableBorders => _gameZoneBorders;
    public int Damage => _damage;

    public float LaunchTime => _launchTime;

    public float HomingTime => _homingTime;
}
