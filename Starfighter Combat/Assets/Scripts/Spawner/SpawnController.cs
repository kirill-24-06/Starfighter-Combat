using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Threading;
using System.Collections.Generic;

public class SpawnController : MonoBehaviour
{
    private SpawnerData _data;

    private Spawner _spawner;

    private CancellationTokenSource _newStageToken;

    private int _activeEnemies = 0;
    private int _activeEliteEnemies = 0;

    private bool _bonusIsActive = false;

    private bool _isGameActive;

    public void Initialise()
    {
        _spawner = EntryPoint.Instance.Spawner;

        EntryPoint.Instance.Events.Start += OnStart;
        EntryPoint.Instance.Events.EnemyDestroyed += OnEnemyDestroyed;
        EntryPoint.Instance.Events.BonusTaken += OnBonusTaken;
        EntryPoint.Instance.Events.Stop += OnStop;
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
        CancelPreviousStage();

        _data = newData;

        StartSpawnLoop();
    }

    private void StartSpawnLoop()
    {
        if (_data.Enemies.Count > 0)
            EnemySpawner(_newStageToken.Token).Forget();

        if (_data.EliteEnemies.Count > 0)
            EliteEnemySpawner(_newStageToken.Token).Forget();

        if (_data.Bonuses.Count > 0)
            BonusSpawner(_newStageToken.Token).Forget();
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

    public void RespawnPlayer() => Respawn(this.GetCancellationTokenOnDestroy()).Forget();

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
        }

        else if (enemyStrenght == EnemyStrenght.Basic)
        {
            _activeEnemies--;
        }
    }

    private void OnBonusTaken() => _bonusIsActive = false;

    public async UniTaskVoid BossArrival(BossWave bossWave, int delayMilliseconds)
    {
        CancelPreviousStage();

        await UniTask.Delay(delayMilliseconds, cancellationToken: _newStageToken.Token);

        _data = bossWave.SpawnerData;

        foreach (var boss in bossWave.Bosses)
            _spawner.SpawnEnemy(boss);

        StartSpawnLoop();
    }

    private void CancelPreviousStage()
    {
        _newStageToken?.Cancel();
        _newStageToken?.Dispose();

        _newStageToken = new CancellationTokenSource();
    }

    private void OnDestroy()
    {
        _newStageToken?.Cancel();
        _newStageToken?.Dispose();
    }
}