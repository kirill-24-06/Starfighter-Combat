using UnityEngine;

public abstract class Projectile : BasicObject,INukeInteractable
{
    [SerializeField] protected ProjectileData _data;

    private IMover _mover;

    protected virtual void Awake() => _mover = new Mover(transform);

    //protected override void Start()
    //{
    //    base.Start();
    //    EntryPoint.Instance.CollisionMap.RegisterNukeInteractable(GetComponent<Collider2D>(), this);
    //}

    protected void Update()
    {
        Move();
        DeactivateOutOfBounds();
    }

    public void GetDamagedByNuke() => Disable();

    protected override void Move() => _mover.Move(Vector2.up, _data.Speed);

    protected override void Disable() => ObjectPoolManager.ReturnObjectToPool(gameObject);

    protected void DeactivateOutOfBounds()
    {
        if (transform.position.y < -_data.DisableBorders.y)
        {
            Disable();
        }

        if (transform.position.y > _data.DisableBorders.y)
        {
            Disable();
        }

        if (transform.position.x < -_data.DisableBorders.x)
        {
            Disable();
        }

        if (transform.position.x > _data.DisableBorders.x)
        {
            Disable();
        }
    }
}