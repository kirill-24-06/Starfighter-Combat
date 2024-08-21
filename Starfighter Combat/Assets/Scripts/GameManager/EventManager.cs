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
    public Action<int> PlayerDamaged;
    public Action<int> PlayerHealed;
    public Action PlayerDied;

    //Bonuses
    public Action<BonusTag> BonusCollected;
    public Action BonusTaken;
    public Action<bool> Multilaser;
    public Action MultilaserEnd;
    public Action Beam;
    public Action BeamEnd;
    public Action ForceField;
    public Action ForceFieldEnd;
    public Action IonSphereUse;
    public Action DroneDestroyed;

    //Enemies
    public Action<ObjectBehaviour> Fire;
    public Action<GameObject, Transform> LockTarget;


    //Ui
    public Action<int> ChangeHealth;
    public Action<int> BonusAmountUpdate;
    public Action<ObjectBehaviour> EnemyDied;
    public Action<int> AddScore;
    public Action<int> ChangeScore;
}