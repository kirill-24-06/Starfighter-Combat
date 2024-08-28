using UnityEngine;

[CreateAssetMenu(fileName = "New FighterData", menuName = "Config Data/Fighter Data", order = 53)]
public class FighterData : ScriptableObject
{
    [SerializeField] private ObjectTag _tag;

    [SerializeField] private int _health;

    [SerializeField] private float _speed;

    [SerializeField] private int _score;

    [SerializeField] private Vector2 _disableBorders;

    [SerializeField] private EnemyProjectile _enemyProjectile;

    [SerializeField] private float _reloadCountDown;

    public ObjectTag Tag => _tag;

    public int Health => _health;

    public float Speed => _speed;

    public int Score => _score;

    public Vector2 DisableBorders => _disableBorders;

    public EnemyProjectile EnemyProjectile => _enemyProjectile;

    public float ReloadCountDown => _reloadCountDown;   
}
