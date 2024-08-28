using UnityEngine;

public abstract class Projectile : BasicObject
{
    [SerializeField] protected ProjectileData _data;

    private IMover _mover;

    protected void Awake()
    {
        _mover = new Mover(transform);
        EntryPoint.Instance.Events.IonSphereUse += OnIonSphereUse;
    }

    protected void Update()
    {
        Move();
        DeactivateOutOfBounds();
    }

    protected void OnIonSphereUse()
    {
        if (gameObject.activeInHierarchy)
            Disable();
    }


    protected override void Move() => _mover.Move(Vector2.up, _data.Speed);

    protected override void Disable() => ObjectPoolManager.ReturnObjectToPool(gameObject);
   
    protected void DeactivateOutOfBounds()
    {
        if (transform.position.y < -_data.GameZoneBorders.y)
        {
            Disable();
        }

        if (transform.position.y > _data.GameZoneBorders.y)
        {
            Disable();
        }

        if (transform.position.x < -_data.GameZoneBorders.x)
        {
            Disable();
        }

        if (transform.position.x > _data.GameZoneBorders.x)
        {
            Disable();
        }
    }

    protected void OnDestroy() => EntryPoint.Instance.Events.IonSphereUse -= OnIonSphereUse;
    
}