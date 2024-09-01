using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private LevelData _data;

    private SpawnController _spawnController;
    private List<LevelStage> _stages;
    private int currentStage = 0;

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
    }

    private void OnStart()
    {
        _stages[currentStage].StartStage();
    }

    private void Update()
    {
        CheckStageComplete();
    }

    private void CheckStageComplete()
    {
        if (_stages[currentStage].IsStageCompleted)
            AdvanceToNextStage();
    }

    private void AdvanceToNextStage()
    {
        if (currentStage + 1 < _stages.Count)
        {
            currentStage++;
            _stages[currentStage].StartStage();
        }
    }

    private void OnNewStageStart()
    {
        Debug.Log("Stage" +  currentStage);
        _spawnController.OnNewStage(_stages[currentStage].GetData().SpawnerData);
    }
}