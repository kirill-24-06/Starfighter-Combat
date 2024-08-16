using System;

public class EventManager
{
    private EventManager() { }

    private static EventManager _instance;

    public static EventManager GetInstance()
    {
        _instance ??= new EventManager();

        return _instance;
    }

    public Action Start;


    //Player
    public Action<int> PlayerDamaged;
    public Action<int> PlayerHealed;
    public Action PlayerDied;
    public Action<bool> Invunerable;

    //Bonuses
    public Action<BonusTag> BonusCollected;
    public Action Multilaser;
    public Action MultilaserEnd;
    public Action Beam;
    public Action BeamEnd;
    public Action ForceField;
    public Action ForceFieldEnd;
    public Action IonSphereUse;
    public Action Drone;

    //Enemies
    public Action<ObjectBehaviour> Fire;

    public Action<ObjectBehaviour> EnemyDied;
    public Action<int> AddScore;
    public Action ChangeScore;
}