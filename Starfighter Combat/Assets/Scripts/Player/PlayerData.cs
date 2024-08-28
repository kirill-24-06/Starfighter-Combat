using UnityEngine;


[CreateAssetMenu(fileName = "New PlayerData", menuName = "Config Data/Player Data", order = 52)]
public class PlayerData : ScriptableObject
{
    [SerializeField] private ObjectTag _tag;
    [SerializeField] private int _health;
    [SerializeField] private int _maxHealth;
    [SerializeField] private float _speed;
    [SerializeField] private int _ionSpheresStartAmount;
    [SerializeField] private GameObject _weapon;
    [SerializeField] private float _reloadTime;
    [SerializeField] private float _bonusLenght;
    [SerializeField] private Vector2 _gameBorders;


    public ObjectTag Tag => _tag;

    public int Health => _health;

    public int MaxHealth => _maxHealth;

    public float Speed => _speed;

    public GameObject Projectile => _weapon;

    public float ReloadTime => _reloadTime;

    public Vector2 GameZoneBorders => _gameBorders;

    public float BonusTimeLenght => _bonusLenght;

    public int IonSpheresStartAmount => _ionSpheresStartAmount;
}
