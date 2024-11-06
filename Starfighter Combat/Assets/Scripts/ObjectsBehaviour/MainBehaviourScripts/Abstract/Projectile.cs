using UnityEngine;

public abstract class Projectile : MonoBehaviour, INukeInteractable
{
    [SerializeField] protected ProjectileData _data;

    protected GameObject _gameObject;
    protected Transform _transform;

    private IMover _mover;

    protected EventManager _events;

    protected bool _isPooled = true;

    protected virtual void Awake()
    {
        _events = EntryPoint.Instance.Events;

        _gameObject = gameObject;
        _transform = transform;

        _mover = new Mover(_transform);

        PoolMap.SetParrentObject(_gameObject, GlobalConstants.PoolTypesByTag[_data.Tag]);
    }

    protected virtual void OnEnable() => _isPooled = false;
  
    private void Update() => _mover.Move(Vector2.up, _data.Speed);

    public void GetDamagedByNuke()
    {
        if (_isPooled) return;
        _isPooled = true;

        ObjectPool.Get(_data.ExplosionPrefab, _transform.position, _data.ExplosionPrefab.transform.rotation);

        ObjectPool.Release(_gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isPooled) return;
        _isPooled = true;

        ObjectPool.Release(_gameObject);
    }
}