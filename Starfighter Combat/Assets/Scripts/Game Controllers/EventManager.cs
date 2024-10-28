using System;
using UnityEngine;

public class EventManager
{
    //Game
    public Action Start;
    public Action Stop;
    public Action<bool> Pause;
    public Action PrewarmRequired;
    public Action LevelCompleted;
    public Action MainMenuExit;

    //Player
    public Action<bool> Invunerable;
    public Action<int> PlayerDamaged;
    public Action<int> PlayerHealed;
    public Action PlayerRespawn;
    public Action PlayerDied;

    //Bonuses
    public Action<BonusTag> BonusCollected;
    public Action BonusTaken;
    public Action<bool> Multilaser;
    public Action MultilaserEnd;
    public Action ForceField;
    public Action ForceFieldEnd;
    public Action IonSphereUse;
    public Action DroneDestroyed;

    //Enemies
    public Action<EnemyStrenght> EnemyDestroyed;
    //public Action<GameObject, int> EnemyDamaged;
    public Action<GameObject, Transform> LockTarget;

    //Boss
    public Action BossArrival;
    public Action BossDefeated;
    public Action<float> BossDamaged;

    //Abilities
    public Action<bool> BossShiedActive;


    //Ui
    public Action<int> ChangeHealth;
    public Action<int> BonusAmountUpdate;
    public Action<int> AddScore;
    public Action<int> ChangeScore;

}