using UnityEngine;

public class Spawner
{
    private SpawnArea[] _spawnAreas;
    private Player _player;
    private Vector3 _playerStartPosition;

    public void Initialise()
    {
        _player = EntryPoint.Instance.Player;
        _playerStartPosition = _player.transform.position;

        _spawnAreas = GameObject.FindObjectsOfType<SpawnArea>();

        if (_spawnAreas != null)
        {
            for (int i = 0; i < _spawnAreas.Length; i++)
                _spawnAreas[i].Initialise(_spawnAreas[i].name);
        }

        else
            Debug.LogError("Зоны спавна не найденны");
    }

    public void Prewarm(PrewarmableData objectToPrewarm)
    {
        ObjectPool.NewPool(objectToPrewarm.Prefab, objectToPrewarm.PrewarmAmount);
    }

    public GameObject SpawnEnemy(SpawnableData enemyToSpawn)
    {
        var area = SelectSpawnArea(enemyToSpawn.SpawnZones);

        var enemy = ObjectPool.Get(enemyToSpawn.Prefab, area.GenerateSpawnPosition(), area.Rotation);

        return enemy;
    }

    public void SpawnBonus(SpawnableData bonusToSpawn)
    {
        var area = SelectSpawnArea(bonusToSpawn.SpawnZones);

        ObjectPool.Get(bonusToSpawn.Prefab, area.GenerateSpawnPosition(), bonusToSpawn.Prefab.transform.rotation);
    }

    public void SpawnPlayer()
    {
        _player.transform.position = _playerStartPosition;
        _player.gameObject.SetActive(true);

        _player.StartTempInvunrability().Forget();
    }

    private SpawnArea SelectSpawnArea(AreaTag[] spawnZones)
    {
        SpawnArea areaForSpawn = null;
        AreaTag spawnAreaTag = AreaTag.None;

        if (spawnZones.Length > 1)
            spawnAreaTag = spawnZones[Random.Range(0, spawnZones.Length)];

        else if (spawnZones.Length == 1)
            spawnAreaTag = spawnZones[0];

        else
            Debug.LogError("Теги зон спавна не указанны");

        for (int i = 0; i < _spawnAreas.Length; i++)
        {
            if (_spawnAreas[i].Tag == spawnAreaTag)
            {
                areaForSpawn = _spawnAreas[i];
                break;
            }
        }

        return areaForSpawn != null ? areaForSpawn : null;
    }
}