using UnityEngine;

public class Asteroid : Enemy
{
    [SerializeField] private AsteroidData _data;

    private IMover _mover;

    private void Awake()
    {
        Initialise();
    }

    private void OnEnable()
    {
        _health = _data.Health;
    }

    protected override void Initialise()
    {
        _mover = new Mover(transform);
    }

    protected override void Move()
    {
        _mover.Move(Vector2.up,_data.Speed);

        DeactivateOutOfBounds(_data.DisableBorders);
    }

    protected override void Disable()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject);

        _events.AddScore?.Invoke(_data.Score);
    }
}