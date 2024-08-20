using System;
using UnityEngine;

public class EventManager
{
    private EventManager() { }

    private static EventManager _instance;

    public static EventManager GetInstance()
    {
        _instance ??= new EventManager();

        return _instance;
    }

    //Game
    public Action Start;
    public Action Stop;
    public Action Pause;
    public Action LevelCompleted;
    public Action MainMenuExit;

    //Player
    public Action<bool> Invunerable;

    //Bonuses
    public Action<BonusTag> BonusCollected;
    public Action BonusTaken;
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
    public Action<GameObject, Transform> LockTarget;


    //Ui
    public Action<int> PlayerDamaged;
    public Action<int> PlayerHealed;
    public Action PlayerDied;
    public Action<int> BonusAmountUpdate;
    public Action<ObjectBehaviour> EnemyDied;
    public Action<int> AddScore;
    public Action<int> ChangeScore;
}