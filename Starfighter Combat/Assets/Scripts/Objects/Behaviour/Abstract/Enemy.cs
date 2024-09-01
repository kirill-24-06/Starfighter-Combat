using UnityEngine;

public enum EnemyStrenght
{
    None,
    Basic,
    Hard
}

public abstract class Enemy : MonoBehaviour
{
    protected EventManager _events;

    protected int _health;

    public IData Data { get; protected set; }

    protected abstract void Disable();
    protected abstract void Initialise();
    protected abstract void Move();

    protected virtual void Start()
    {
        _events = EntryPoint.Instance.Events;
        _events.IonSphereUse += OnIonSphereUse;
        _events.EnemyDamaged += OnDamaged;
    }

    protected virtual void Update()
    {
        if (!enabled)
            return;

        Move();
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == EntryPoint.Instance.Player.ForceField.gameObject)
            Disable();

        else if (Player.IsPlayer(collision.gameObject))
            Collide();
    }

    private void Collide()
    {
        if (!EntryPoint.Instance.Player.IsDroneActive)
            _events.PlayerDamaged?.Invoke(1);

        else
            _events.DroneDestroyed?.Invoke();

        Disable();
    }

    protected void TakeDamage(int damage)
    {
        if (damage > 0)
        {
            _health -= damage;

            if (_health < 0)
                _health = 0;
        }

        if (_health == 0)
            Disable();
    }

    private void OnIonSphereUse()
    {
        if (gameObject.activeInHierarchy)
            TakeDamage(10);
    }

    protected void DeactivateOutOfBounds(Vector2 bounds)
    {
        if (transform.position.y < -bounds.y)
            Deactivate();

        if (transform.position.y > bounds.y)
            Deactivate();

        if (transform.position.x < -bounds.x)
            Deactivate();

        if (transform.position.x > bounds.x)
            Deactivate();
    }

    protected void OnDamaged(GameObject enemy, int damage)
    {
        if (enemy == gameObject)
            TakeDamage(damage);
    }

    private void Deactivate()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject);
        _events.EnemyDestroyed?.Invoke(Data.EnemyStrenght);
    }

    protected void OnDestroy() => _events.IonSphereUse -= OnIonSphereUse;
}