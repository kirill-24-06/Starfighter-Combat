using UnityEngine;

public enum EnemyStrenght
{
    None,
    Basic,
    Hard
}

public abstract class Enemy : MonoBehaviour, IInteractableEnemy,INukeInteractable
{
    protected EventManager _events;

    protected int _health;

    public IData Data { get; protected set; }

    protected abstract void Initialise();
    protected abstract void Move();
    protected abstract void Disable();

    protected virtual void Awake() => _events = EntryPoint.Instance.Events;

    //protected virtual void Start()
    //{
    //    var collider = GetComponent<Collider2D>();
    //    EntryPoint.Instance.CollisionMap.Register(collider,this);
    //    EntryPoint.Instance.CollisionMap.RegisterNukeInteractable(collider, this);
    //    EntryPoint.Instance.MissileTargets.AddEnemy(transform);
    //}

   
    protected virtual void Update()
    {
        Move();
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (Player.IsPlayer(collision.gameObject) || collision.gameObject == EntryPoint.Instance.Player.ForceField.gameObject)
            Collide();
    }

    protected virtual void Collide()
    {
        if (!EntryPoint.Instance.Player.IsInvunerable && !EntryPoint.Instance.Player.IsDroneActive)
            _events.PlayerDamaged?.Invoke(GlobalConstants.CollisionDamage);

        else if (!EntryPoint.Instance.Player.IsInvunerable && EntryPoint.Instance.Player.IsDroneActive)
            _events.DroneDestroyed?.Invoke();

        Disable();
    }

    public void Interact() => TakeDamage(GlobalConstants.CollisionDamage);
   
    public void TakeDamage(int damage)
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

    public void GetDamagedByNuke() => TakeDamage(GlobalConstants.NukeDamage);
    
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

    private void Deactivate()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject);
        _events.EnemyDestroyed?.Invoke(Data.EnemyStrenght);
    }
}