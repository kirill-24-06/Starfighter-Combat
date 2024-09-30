using System.Collections;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    private SpawnerData _data;

    private Spawner _spawner;

    private IEnumerator _enemySpawner;
    private IEnumerator _hardEnemySpawner;
    private IEnumerator _bonusSpawner;

    private int _activeEnemies = 0;
    private int _activeHardEnemies = 0;

    private bool _bonusIsActive = false;

    private bool _isGameActive;

    public void Initialise()
    {
        _spawner = EntryPoint.Instance.Spawner;

        _enemySpawner = EnemySpawner();
        _hardEnemySpawner = HardEnemySpawner();
        _bonusSpawner = BonusSpawner();

        EntryPoint.Instance.Events.Start += OnStart;
        EntryPoint.Instance.Events.EnemyDestroyed += OnEnemyDestroyed;
        EntryPoint.Instance.Events.BonusTaken += OnBonusTaken;
        EntryPoint.Instance.Events.Stop += OnStop;
    }

    private void OnStart() => _isGameActive = true;

    private void OnStop() => _isGameActive = false;

    public void NewStage(SpawnerData newData)
    {
        StopAllCoroutines();

        _data = newData;

        if (_data.Enemies.Count > 0)
            StartCoroutine(_enemySpawner);
     
        if (_data.HardEnemies.Count > 0)
            StartCoroutine(_hardEnemySpawner);
        
        if (_data.Bonuses.Count > 0)
            StartCoroutine(_bonusSpawner);
    }

    private void SpawnEnemy()
    {
        if (_activeEnemies >= _data.MaxEnemies)
            return;

        _spawner.SpawnEnemy(_data.Enemies[Random.Range(0, _data.Enemies.Count)]);
        _activeEnemies++;
    }

    private void SpawnHardEnemy()
    {
        if (_activeEnemies >= _data.MaxEnemies || _activeHardEnemies >= _data.MaxHardEnemies)
            return;

        _spawner.SpawnEnemy(_data.HardEnemies[Random.Range(0, _data.HardEnemies.Count)]);
        _activeEnemies++;
        _activeHardEnemies++;
    }

    private void SpawnBonus()
    {
        if (_bonusIsActive)
            return;

        _spawner.SpawnBonus(_data.Bonuses[Random.Range(0, _data.Bonuses.Count)]);
        _bonusIsActive = true;
    }

    public void RespawnPlayer() => StartCoroutine(Respawn());
    

    //bug
    private IEnumerator Respawn()
    {
        float count = 0;

        while (count < 2.0f)
        {
            count += Time.deltaTime;

            yield return null;
        }

        _spawner.SpawnPlayer();
    }

    private void OnEnemyDestroyed(EnemyStrenght enemyStrenght)
    {
        if (enemyStrenght == EnemyStrenght.Hard)
        {
            _activeEnemies--;
            _activeHardEnemies--;
        }

        else if (enemyStrenght == EnemyStrenght.Basic)
        {
            _activeEnemies--;
        }
    }

    private void OnBonusTaken() => _bonusIsActive = false;


    public void BossArrival(BossWave bossWave)
    {
        StopAllCoroutines();

        _data = bossWave.SpawnerData;

        foreach (var boss in bossWave.Bosses)
        {
            _spawner.SpawnEnemy(boss);
        }

        if (_data.Enemies.Count > 0)
            StartCoroutine(_enemySpawner);

        if (_data.HardEnemies.Count > 0)
            StartCoroutine(_hardEnemySpawner);

        if (_data.Bonuses.Count > 0)
            StartCoroutine(_bonusSpawner);
    }


    private IEnumerator EnemySpawner()
    {
        float count = 0;
        yield return new WaitForSeconds(_data.SpawnDelay);

        while (_isGameActive)
        {
            count += Time.deltaTime;

            if (count >= _data.SpawnTime)
            {
                SpawnEnemy();
                count = 0;
            }

            yield return null;
        }
    }

    private IEnumerator HardEnemySpawner()
    {
        float count = 0;
        yield return new WaitForSeconds(_data.SpawnDelay);

        while (_isGameActive)
        {
            count += Time.deltaTime;

            if (count >= _data.SpawnTime * 2)
            {
                SpawnHardEnemy();
                count = 0;
            }

            yield return null;
        }
    }

    private IEnumerator BonusSpawner()
    {
        float count = 0;
        yield return new WaitForSeconds(_data.SpawnDelay);

        while (_isGameActive)
        {
            count += Time.deltaTime;

            if (count >= _data.BonusSpawnTime)
            {
                if (!_bonusIsActive)
                {
                    SpawnBonus();
                    _bonusIsActive = true;
                    count = 0;
                }

                else
                {
                    count = 0;
                }
            }

            yield return null;
        }
    }
}