using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private LevelData _data;

    private SpawnController _spawnController;

    private List<LevelStage> _stages;
    private int currentStage = 0;

    private bool _isGameActive;
    private bool _isBossFight = false;
    private bool _isBossDefeated = false;


    public void Initialise()
    {
        _spawnController = EntryPoint.Instance.SpawnController;

        _stages = new List<LevelStage>(_data.Stages.Count);

        for (int i = 0; i < _data.Stages.Count; i++)
        {
            _stages.Add(new LevelStage(this, _data.Stages[i]));
            _stages[i].StageStart += OnNewStageStart;
        }

        EntryPoint.Instance.Events.Start += OnStart;
        EntryPoint.Instance.Events.Stop += OnStop;
        EntryPoint.Instance.Events.BossDefeated += OnBossDefeat;
    }

    private void OnStart()
    {
        _isGameActive = true;
        _stages[currentStage].StartStage();
    }

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

            EntryPoint.Instance.Events.LevelCompleted?.Invoke();
        }

        else if (_stages[currentStage].IsStageCompleted)
            AdvanceToNextStage();
    }

    private void OnBossDefeat() => _isBossDefeated = true;

    private void OnStop() => _isGameActive = false;

    private void AdvanceToNextStage()
    {
        if (currentStage + 1 < _stages.Count)
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
}