using UnityEngine;

public class Asteroid : Enemy
{
    [SerializeField] private AsteroidData _data;

    private IMover _mover;

    protected override void Awake()
    {
        base.Awake();

        Initialise();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _health = _data.Health;
    }

    protected override void Initialise()
    {
        _mover = new Mover(transform);

        Data = _data;
    }

    protected override void Move()
    {
        _mover.Move(Vector2.up,_data.Speed);

        DeactivateOutOfBounds(_data.DisableBorders);
    }

    protected override void Disable()
    {
        Instantiate(_data.Explosion, transform.position, _data.Explosion.transform.rotation);

        ObjectPoolManager.ReturnObjectToPool(gameObject);

        _events.AddScore?.Invoke(_data.Score);
        _events.EnemyDestroyed?.Invoke(_data.EnemyStrenght);
    }
}