using System.Collections.Generic;
using UnityEngine;

public class Spawner
{
    private List<SpawnArea> _spawnAreas;
    private ObjectHolder _spawnedObjects;
    private Player _player;
    private Vector3 _playerStartPosition;


    public void Initialise()
    {
        _player = EntryPoint.Instance.Player;
        _playerStartPosition = _player.transform.position;

        _spawnAreas = new List<SpawnArea>();

        SpawnArea[] existedAreas = GameObject.FindObjectsOfType<SpawnArea>();

        if (existedAreas != null)
        {
            foreach (var item in existedAreas)
            {
                item.Initialise(item.name);
                _spawnAreas.Add(item);
            }
        }

        else
            Debug.LogError("Зоны спавна не найденны");


        _spawnedObjects = EntryPoint.Instance.SpawnedObjects;
    }



    public void SpawnEnemy(SpawnableData enemyToSpawn)
    {
        SpawnArea area = SelectSpawnArea(enemyToSpawn.SpawnZones);

        GameObject spawnedObject =  ObjectPoolManager.SpawnObject(enemyToSpawn.Prefab, area.GenerateSpawnPosition(), area.Rotation, ObjectPoolManager.PoolType.Enemy);
        _spawnedObjects.RegisterObject(spawnedObject, ObjectTag.Enemy);
    }


    public void SpawnBonus(SpawnableData bonusToSpawn)
    {
        SpawnArea area = SelectSpawnArea(bonusToSpawn.SpawnZones);

        GameObject spawnedObject = ObjectPoolManager.SpawnObject(bonusToSpawn.Prefab, area.GenerateSpawnPosition(), bonusToSpawn.Prefab.transform.rotation, ObjectPoolManager.PoolType.Bonus);
        _spawnedObjects.RegisterObject(spawnedObject, ObjectTag.Bonus);
    }

    public void SpawnPlayer()
    {
        _player.transform.position = _playerStartPosition;
        _player.gameObject.SetActive(true);

        _player.ActivateForceField();
    }

    private SpawnArea SelectSpawnArea(AreaTag[] spawnZones)
    {
        AreaTag spawnAreaTag = AreaTag.None;

        if (spawnZones.Length > 1)
            spawnAreaTag = spawnZones[Random.Range(0, spawnZones.Length)];

        else if (spawnZones.Length == 1)
            spawnAreaTag = spawnZones[0];

        else
            Debug.LogError("Теги зон спавна не указанны");

        SpawnArea spawnArea = _spawnAreas.Find(spawnArea => spawnArea.Tag == spawnAreaTag);

        if (spawnArea != null)
            return spawnArea;

        else
        {
            Debug.LogError("Зона с таким тегом отстутствует");
            return null;
        }
    }
}