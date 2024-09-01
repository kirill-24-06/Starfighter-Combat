using UnityEngine;

public interface IData
{
    public Vector2 DisableBorders {  get;}

    public float Speed { get;}

    public AreaTag[] SpawnZones { get;}

    public EnemyStrenght EnemyStrenght { get;}
}
