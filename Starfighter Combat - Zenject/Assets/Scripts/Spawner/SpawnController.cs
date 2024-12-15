using Cysharp.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System;

using Random = UnityEngine.Random;

public class SpawnController : IDisposable
{
    private SpawnerData _data;

    private Spawner _spawner;

    private EventManager _events;

    private CancellationTokenSource[] _stageTokens;
    private CancellationTokenSource _sceneExitToken;

    private int _currentStage = 0;

    private int _activeEnemies = 0;
    private int _activeEliteEnemies = 0;

    private bool _bonusIsActive = false;

    private bool _isGameActive;

    public SpawnController(Spawner spawner, EventManager events, int stagesAmount = 8)
    {
        _spawner = spawner;
        _events = events;

        _stageTokens = new CancellationTokenSource[stagesAmount + 1];
        for (int i = 0; i < _stageTokens.Length; i++)
        {
            _stageTokens[i] = new CancellationTokenSource();
        }

        _sceneExitToken = new CancellationTokenSource();

        _events.Start += OnStart;
        _events.EnemyDestroyed += OnEnemyDestroyed;
        _events.BonusTaken += OnBonusTaken;
        _events.Stop += OnStop;
    }

    public SpawnController Prewarm(List<PrewarmableData> prewarmables)
    {
        foreach (var prewarmable in prewarmables)
            _spawner.Prewarm(prewarmable);

        return this;
    }

    private void OnStart() => _isGameActive = true;

    private void OnStop() => _isGameActive = false;

    public void NewStage(SpawnerData newData)
    {
        if (_currentStage != 0)
            CancelPreviousStage();

        _data = newData;

        StartSpawnLoop();
    }

    private void StartSpawnLoop()
    {
        if (_data.Enemies.Count > 0)
            EnemySpawner(_stageTokens[_currentStage].Token).Forget();

        if (_data.EliteEnemies.Count > 0)
            EliteEnemySpawner(_stageTokens[_currentStage].Token).Forget();

        if (_data.Bonuses.Count > 0)
            BonusSpawner(_stageTokens[_currentStage].Token).Forget();

        _currentStage++;
    }

    private async UniTaskVoid EnemySpawner(CancellationToken token)
    {
        await UniTask.Delay(_data.SpawnDelay, cancellationToken: token);

        while (_isGameActive)
        {
            SpawnEnemy();

            await UniTask.Delay(_data.SpawnTime, cancellationToken: token);
        }
    }

    private async UniTaskVoid EliteEnemySpawner(CancellationToken token)
    {
        await UniTask.Delay(_data.SpawnDelay, cancellationToken: token);

        while (_isGameActive)
        {
            SpawnEliteEnemy();

            await UniTask.Delay(_data.EliteSpawnTime, cancellationToken: token);
        }
    }

    private async UniTaskVoid BonusSpawner(CancellationToken token)
    {
        await UniTask.Delay(_data.SpawnDelay, cancellationToken: token);

        while (_isGameActive)
        {
            if (!_bonusIsActive)
            {
                SpawnBonus();
                _bonusIsActive = true;
            }
            await UniTask.Delay(_data.BonusSpawnTime, cancellationToken: token);
        }
    }

    private void SpawnEnemy()
    {
        if (_activeEnemies >= _data.MaxEnemies)
            return;

        _spawner.SpawnEnemy(_data.Enemies[Random.Range(0, _data.Enemies.Count)]);
        _activeEnemies++;
    }

    private void SpawnEliteEnemy()
    {
        if (_activeEnemies >= _data.MaxEnemies || _activeEliteEnemies >= _data.MaxHardEnemies)
            return;

        _spawner.SpawnEnemy(_data.EliteEnemies[Random.Range(0, _data.EliteEnemies.Count)]);
        _activeEnemies++;
        _activeEliteEnemies++;
    }

    private void SpawnBonus()
    {
        if (_bonusIsActive)
            return;

        _spawner.SpawnBonus(_data.Bonuses[Random.Range(0, _data.Bonuses.Count)]);
        _bonusIsActive = true;
    }

    public void RespawnPlayer() => Respawn(_sceneExitToken.Token).Forget();

    private async UniTaskVoid Respawn(CancellationToken cancellationToken)
    {
        await UniTask.Delay(2000, cancellationToken: cancellationToken);
        _spawner.SpawnPlayer();
    }

    private void OnEnemyDestroyed(EnemyStrenght enemyStrenght)
    {
        if (enemyStrenght == EnemyStrenght.Hard)
        {
            _activeEnemies--;
            _activeEliteEnemies--;

            if (_activeEliteEnemies < 0)
                _activeEliteEnemies = 0;
        }

        else if (enemyStrenght == EnemyStrenght.Basic)
            _activeEnemies--;

        if (_activeEnemies < 0)
            _activeEnemies = 0;
    }

    private void OnBonusTaken() => _bonusIsActive = false;

    public async UniTaskVoid BossArrival(BossWave bossWave, int delayMilliseconds)
    {
        CancelPreviousStage();

        await UniTask.Delay(delayMilliseconds, cancellationToken: _sceneExitToken.Token);

        _data = bossWave.SpawnerData;

        foreach (var boss in bossWave.Bosses)
            _spawner.SpawnEnemy(boss);

        StartSpawnLoop();
    }

    private void CancelPreviousStage()
    {
        _stageTokens[_currentStage - 1].Cancel();
        _stageTokens[_currentStage - 1].Dispose();
    }

    public void Dispose()
    {
        foreach (var token in _stageTokens)
        {
            token?.Cancel();
            token?.Dispose();
        }

        _sceneExitToken?.Cancel();
        _sceneExitToken?.Dispose();

        _events.Start -= OnStart;
        _events.EnemyDestroyed -= OnEnemyDestroyed;
        _events.BonusTaken -= OnBonusTaken;
        _events.Stop -= OnStop;
    }
}