using UnityEngine;

public abstract class Projectile : MonoBehaviour,INukeInteractable
{
    [SerializeField] protected ProjectileData _data;

    protected GameObject _gameObject;
    protected Transform _transform;

    private IMover _mover;

    protected EventManager _events;

    protected virtual void Awake()
    {
        _events = EntryPoint.Instance.Events;

        _gameObject = gameObject;
        _transform = transform;

        _mover = new Mover(_transform);

        PoolMap.SetParrentObject(GlobalConstants.PoolTypesByTag[_data.Tag]);
    }

    private void Update() => _mover.Move(Vector2.up, _data.Speed);

    public void GetDamagedByNuke() => ObjectPool.Release(gameObject);

    private void OnTriggerEnter2D(Collider2D collision) => ObjectPool.Release(_gameObject);
}