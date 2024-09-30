using System;

public class LevelStage
{
    private readonly StageData _data;// <= заменить на SpawnerData

    private readonly Timer _stageTimer;
    public bool IsStageCompleted { get; private set; }

    public Action StageStart;

    public LevelStage(LevelController levelController, StageData data)
    {
        _data = data;
        IsStageCompleted = false;

        _stageTimer = new Timer(levelController);
        _stageTimer.SetTimer(data.StageTime);
        _stageTimer.TimeIsOver += () => { IsStageCompleted = true; };
    }
    
    public void StartStage()
    {
        StageStart?.Invoke();
        _stageTimer.StartTimer();
    }

    public StageData GetData() => _data; 
}
