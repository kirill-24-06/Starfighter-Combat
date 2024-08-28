using UnityEngine;


[CreateAssetMenu(fileName = "New AsteroidData", menuName = "Config Data/Asteroid Data", order = 53)]
public class AsteroidData : ScriptableObject,IData
{
    [SerializeField] private ObjectTag _tag;

    [SerializeField] private int _health;

    [SerializeField] private float _speed;

    [SerializeField] private int _score;

    [SerializeField] private Vector2 _disableBorders;

    public ObjectTag Tag =>_tag;

    public int Health => _health;

    public float Speed => _speed;

    public int Score => _score;

    public Vector2 DisableBorders => _disableBorders;
}