using System.Collections.Generic;
using UnityEngine;

public class Spawner
{
    private List<SpawnArea> _spawnAreas;
    //private ObjectHolder _spawnedObjects;
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


        //_spawnedObjects = EntryPoint.Instance.SpawnedObjects;
    }

    public void Prewarm(PrewarmableData objectToPrewarm)
    {
        for (int i = 0; i < objectToPrewarm.PrewarmAmount; i++)
        {
            ObjectPoolManager.Prewarm(objectToPrewarm.Prefab, GlobalConstants.PoolTypesByTag[objectToPrewarm.Tag]);
            //_spawnedObjects.RegisterObject(spawnedObject,objectToPrewarm.Tag);
        }
    }

    public GameObject SpawnEnemy(SpawnableData enemyToSpawn)
    {
        SpawnArea area = SelectSpawnArea(enemyToSpawn.SpawnZones);

        var enemy = ObjectPoolManager.SpawnObject(enemyToSpawn.Prefab, area.GenerateSpawnPosition(),
            area.Rotation, ObjectPoolManager.PoolType.Enemy);

        return enemy;
    }


    public void SpawnBonus(SpawnableData bonusToSpawn)
    {
        SpawnArea area = SelectSpawnArea(bonusToSpawn.SpawnZones);

        ObjectPoolManager.SpawnObject(bonusToSpawn.Prefab, area.GenerateSpawnPosition(),
            bonusToSpawn.Prefab.transform.rotation, ObjectPoolManager.PoolType.Bonus);
    }

    public void SpawnPlayer()
    {
        _player.transform.position = _playerStartPosition;
        _player.gameObject.SetActive(true);

        _player.StartTempInvunrability().Forget();
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