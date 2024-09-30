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

    protected virtual void Awake() => _events = EntryPoint.Instance.Events;
   
    protected virtual void OnEnable()
    {
        _events.IonSphereUse += OnIonSphereUse;
        _events.EnemyDamaged += OnDamaged;
    }

    protected virtual void Update() => Move();
   
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (Player.IsPlayer(collision.gameObject) || collision.gameObject == EntryPoint.Instance.Player.ForceField.gameObject)
            Collide();
    }

    private void Collide()
    {
        if (!EntryPoint.Instance.Player.IsInvunerable && !EntryPoint.Instance.Player.IsDroneActive)
            _events.PlayerDamaged?.Invoke(GlobalConstants.CollisionDamage);

        else if (!EntryPoint.Instance.Player.IsInvunerable && EntryPoint.Instance.Player.IsDroneActive)
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

    protected virtual void OnIonSphereUse()
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
    protected virtual void OnDisable()
    {
        _events.IonSphereUse -= OnIonSphereUse;
        _events.EnemyDamaged -= OnDamaged;
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
}