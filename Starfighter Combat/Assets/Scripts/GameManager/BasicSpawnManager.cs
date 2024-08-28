using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSpawnManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _objectsToSpawn;
    [SerializeField] private List<GameObject> _bonusesToSpawn;
    private ObjectHolder _spawnedObjects;

    [SerializeField] private SpriteRenderer _spawnRenderer;

    [SerializeField] private float _spawnDelay;
    [SerializeField] private float _spawnTime;
    [SerializeField] private float _bonusSpawnTime;

    private Bounds _spawnArea;
    private readonly Quaternion _rotation = Quaternion.Euler(0, 0, 180);

    private IEnumerator _spawner;
    private IEnumerator _bonusSpawner;

    private bool _isGameActive = false;
    private bool _bonusIsSpawn = false;


    public void Initialise()
    {
        _spawnedObjects = EntryPoint.Instance.SpawnedObjects;
        _spawnArea = _spawnRenderer.bounds;

        _spawner = Spawner();
        _bonusSpawner = BonusSpawner();

        EntryPoint.Instance.Events.BonusTaken += OnBonusTaken;
        EntryPoint.Instance.Events.Start += OnStart;
        EntryPoint.Instance.Events.Stop += OnStop;
    }

    private void OnStart()
    {
        _isGameActive = true;

        StartCoroutine(_spawner);
        StartCoroutine(_bonusSpawner);
    }

    private IEnumerator Spawner()
    {
        float count = 0;
        yield return new WaitForSeconds(_spawnDelay);

        while (_isGameActive)
        {
            count += Time.deltaTime;

            if (count >= _spawnTime)
            {
                SpawnEnemy();
                count = 0;
            }

            yield return null;
        }
    }

    private IEnumerator BonusSpawner()
    {
        float count = 0;
        yield return new WaitForSeconds(_spawnDelay);

        while (_isGameActive)
        {
            count += Time.deltaTime;

            if (count >= _bonusSpawnTime)
            {
                if (!_bonusIsSpawn)
                {
                    SpawnBonus();
                    _bonusIsSpawn = true;
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

    private void OnStop()
    {
        _isGameActive = false;
    }

    private void OnDestroy()
    {
        _isGameActive = false;

        EntryPoint.Instance.Events.BonusTaken -= OnBonusTaken;
        EntryPoint.Instance.Events.Start -= OnStart;
        EntryPoint.Instance.Events.Stop -= OnStop;
    }

    private void SpawnEnemy()
    {
        int enemyIndex = Random.Range(0, _objectsToSpawn.Count);
        GameObject enemy;

        enemy = ObjectPoolManager.SpawnObject(_objectsToSpawn[enemyIndex], GenerateSpawnPosition(), _rotation, ObjectPoolManager.PoolType.Enemy);
        _spawnedObjects.RegisterObject(enemy, ObjectTag.Enemy);
    }

    private void SpawnBonus()
    {
        int bonusIndex = Random.Range(0, _bonusesToSpawn.Count);
        GameObject bonus;

        bonus = ObjectPoolManager.SpawnObject(_bonusesToSpawn[bonusIndex], GenerateSpawnPosition(), _rotation, ObjectPoolManager.PoolType.Bonus);
        _spawnedObjects.RegisterObject(bonus, ObjectTag.Bonus);
    }

    private void OnBonusTaken()
    {
        _bonusIsSpawn = false;
    }

    private Vector3 GenerateSpawnPosition()
    {
        float randomX = Random.Range(_spawnArea.min.x, _spawnArea.max.x);
        float randomY = Random.Range(_spawnArea.min.y, _spawnArea.max.y);
        float randomZ = 0;

        return new Vector3(randomX, randomY, randomZ);
    }
}