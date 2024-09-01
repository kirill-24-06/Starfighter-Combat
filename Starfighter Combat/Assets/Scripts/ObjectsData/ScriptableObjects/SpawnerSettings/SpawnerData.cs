using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New SpawnerData", menuName = "Config Data/Level/Spawner Data", order = 53)]
public class SpawnerData : ScriptableObject
{
    [SerializeField] private List<SpawnableData> _enemies;
    [SerializeField] private List<SpawnableData> _hardEnemies;
    [SerializeField] private List<SpawnableData> _bonuses;

    [SerializeField] private float _spawnDelay;
    [SerializeField] private float _spawnTime;
    [SerializeField] private float _bonusSpawnTime;

    [SerializeField] private int _maxEnemies;

    [SerializeField] private int _maxHardEnemies;


    public List<SpawnableData> Enemies => _enemies;

    public List<SpawnableData> HardEnemies => _hardEnemies;

    public List<SpawnableData> Bonuses => _bonuses;

    public float SpawnDelay => _spawnDelay;

    public float SpawnTime => _spawnTime;

    public float BonusSpawnTime=> _bonusSpawnTime;

    public int MaxEnemies => _maxEnemies;

    public int MaxHardEnemies => _maxHardEnemies;
}