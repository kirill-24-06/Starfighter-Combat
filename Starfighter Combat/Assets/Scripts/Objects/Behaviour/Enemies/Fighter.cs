using UnityEngine;

public class Fighter : Enemy
{
    [SerializeField] private FighterData _data;

    private EnemyAttacker _attacker;

    private IMover _mover;

    private void Awake() => Initialise();

    private void OnEnable()
    {
        _health = _data.Health;
        _attacker.Reset();
    }

    protected override void Update()
    {
        base.Update();

        _attacker.Fire(_data.EnemyProjectile.gameObject);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    protected override void Initialise()
    {
        _mover = new Mover(transform);
        _attacker = new EnemyAttacker(this, _data.ReloadCountDown);

        Data = _data;
    }

    protected override void Move()
    {
        _mover.Move(Vector2.up, _data.Speed);

        DeactivateOutOfBounds(_data.DisableBorders);
    }
    protected override void Disable()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject);

        _events.AddScore?.Invoke(_data.Score);
        _events.EnemyDestroyed?.Invoke(_data.EnemyStrenght);
    }
}