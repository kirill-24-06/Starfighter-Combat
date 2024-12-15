using System;
using Zenject;

public class LevelController : IDisposable, ITickable
{
    private LevelData _data;

    private SpawnController _spawnController;
    private EventManager _events;

    private LevelStage[] _stages;
    private int currentStage = 0;

    private bool _isGameActive;
    private bool _isBossFight = false;
    private bool _isBossDefeated = false;

    public LevelController(LevelData data, LevelStage[] stages, SpawnController spawnController, EventManager events)
    {
        _data = data;
        _spawnController = spawnController;
        _events = events;

        _stages = stages;

        for (int i = 0; i < _data.Stages.Count; i++)
            _stages[i].StageStart += OnNewStageStart;


        _events.Start += OnStart;
        _events.Stop += OnStop;
        _events.BossDefeated += OnBossDefeat;
        _events.PrewarmRequired += OnPrewarmRequire;
    }

    private void OnStart()
    {
        _isGameActive = true;
        _stages[currentStage].StartStage();
    }

    private void OnPrewarmRequire() => _spawnController.Prewarm(_data.Prewarmables);

    public void Tick() => Update();
   
    private void Update()
    {
        if (!_isGameActive)
            return;

        CheckLevelCompletion();
    }

    private void CheckLevelCompletion()
    {
        if (_isBossFight)
        {
            if (!_isBossDefeated)
                return;

            _events.LevelCompleted?.Invoke();
        }

        else if (_stages[currentStage].IsStageCompleted)
            AdvanceToNextStage();
    }

    private void OnBossDefeat() => _isBossDefeated = true;

    private void OnStop() => _isGameActive = false;

    private void AdvanceToNextStage()
    {
        if (currentStage + 1 < _stages.Length)
        {
            currentStage++;
            _stages[currentStage].StartStage();
        }

        else
        {
            _isBossFight = true;
            _spawnController.BossArrival(_data.BossWave, _data.BossWaveDelay).Forget();
        }
    }

    private void OnNewStageStart()
    {
        _spawnController.NewStage(_stages[currentStage].GetData().SpawnerData);
    }

    public void Dispose()
    {
        _events.Start -= OnStart;
        _events.Stop -= OnStop;
        _events.BossDefeated -= OnBossDefeat;
        _events.PrewarmRequired -= OnPrewarmRequire;
    }
}