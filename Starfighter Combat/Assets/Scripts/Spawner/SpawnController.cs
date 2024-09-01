using System.Collections;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private SpawnerData _data;

    private Spawner _spawner;

    //ToDo
    //private Timer _enemySpawnTimer;
    //private Timer _hardSpawnTimer;
    //private Timer _bonusSpawnTimer;

    //Temp
    private IEnumerator _enemySpawner;
    private IEnumerator _hardEnemySpawner;
    private IEnumerator _bonusSpawner;

    private int _activeEnemies = 0;
    private int _activeHardEnemies = 0;

    private bool _bonusIsActive = false;


    private bool _isGameActive;


    public void Initialise()
    {
        //_data =

        _spawner = EntryPoint.Instance.Spawner;

        //_enemySpawnTimer = new Timer(this);
        //_hardSpawnTimer = new Timer(this);
        //_bonusSpawnTimer = new Timer(this);

        _enemySpawner = EnemySpawner();
        _hardEnemySpawner = HardEnemySpawner();
        _bonusSpawner = BonusSpawner();

        //SetTimers();

        EntryPoint.Instance.Events.Start += OnStart;
        EntryPoint.Instance.Events.EnemyDestroyed += OnEnemyDestroyed;
        EntryPoint.Instance.Events.BonusTaken += OnBonusTaken;
        EntryPoint.Instance.Events.Stop += OnStop;

    }

    
    private void OnStart()
    {
        //yield return new WaitForSeconds(_data.SpawnDelay);     
        _isGameActive = true;

        if (_data.Enemies.Count > 0)
            StartCoroutine(_enemySpawner);
            //_enemySpawnTimer.StartTimer();

        if (_data.HardEnemies.Count > 0) //<==Debug
            StartCoroutine(_hardEnemySpawner);
            //_hardSpawnTimer.StartTimer();

        if (_data.Bonuses.Count > 0)
            StartCoroutine(_bonusSpawner);
            //_bonusSpawnTimer.StartTimer();
    }

    private void OnStop() => _isGameActive = false;

    public void OnNewStage(SpawnerData newData)
    {
        StopAllCoroutines();

        _data = newData;

        if (_data.Enemies.Count > 0)
            StartCoroutine(_enemySpawner);
        //_enemySpawnTimer.StartTimer();

        if (_data.HardEnemies.Count > 0) 
            StartCoroutine(_hardEnemySpawner);
        //_hardSpawnTimer.StartTimer();

        if (_data.Bonuses.Count > 0)
            StartCoroutine(_bonusSpawner);
        //_bonusSpawnTimer.StartTimer();
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


    private void OnBossArrival()
    {
        //ToDo
        StopAllCoroutines();
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

    //private void SetTimers()
    //{
    //    _enemySpawnTimer.SetTimer(_data.SpawnTime);
    //    _hardSpawnTimer.SetTimer(_data.SpawnTime * 2);
    //    _bonusSpawnTimer.SetTimer(_data.BonusSpawnTime);

    //    _enemySpawnTimer.TimeIsOver += () =>
    //    {
    //        if (_isGameActive)
    //        {
    //            SpawnEnemy();
    //           _enemySpawnTimer.StartTimer();
    //        }
    //    };

    //    _hardSpawnTimer.TimeIsOver += () =>
    //    {
    //        if (_isGameActive)
    //        {
    //            SpawnHardEnemy();
    //            _hardSpawnTimer.StartTimer();
    //        }
    //    };

    //    _bonusSpawnTimer.TimeIsOver += () =>
    //    {
    //        if (_isGameActive)
    //        {
    //            SpawnBonus();
    //            _bonusSpawnTimer.StartTimer();
    //        }
    //    };
    //}


}
