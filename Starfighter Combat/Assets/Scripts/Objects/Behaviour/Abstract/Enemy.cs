using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected EventManager _events;

    protected int _health;

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

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == EntryPoint.Instance.Player.ForceField.gameObject)
        {
            Disable();
        }

        else if (Player.IsPlayer(collision.gameObject))
        {
            Collide();
        }
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
        {
            TakeDamage(10);
        }
    }

    protected void DeactivateOutOfBounds(Vector2 bounds)
    {
        if (transform.position.y < -bounds.y)
        {
            Disable();
        }

        if (transform.position.y > bounds.y)
        {
            Disable();
        }

        if (transform.position.x < -bounds.x)
        {
            Disable();
        }

        if (transform.position.x > bounds.x)
        {
            Disable();
        }
    }

    protected void OnDamaged(GameObject enemy, int damage)
    {
        if (enemy == gameObject)
        {
            TakeDamage(damage);
        }
    }

    protected void OnDestroy()
    {
        _events.IonSphereUse -= OnIonSphereUse;
    }
}