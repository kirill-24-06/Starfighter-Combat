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

    public Action<int> PlayerDamaged;
    public Action PlayerDied;

    public Action<ObjectBehaviour> EnemyDied;

    public Action<int> AddScore;
    public Action ChangeScore;
}