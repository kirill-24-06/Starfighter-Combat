using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New SpawnerData", menuName = "Config Data/Level/Spawner Data", order = 53)]
public class SpawnerData : ScriptableObject
{
    [SerializeField] private List<SpawnableData> _enemies;
    [SerializeField] private List<SpawnableData> _eliteEnemies;
    [SerializeField] private List<SpawnableData> _bonuses;

    [SerializeField] private float _spawnDelay;
    [SerializeField] private float _spawnTime;
    [SerializeField] private float _eliteSpawnTime;
    [SerializeField] private float _bonusSpawnTime;

    [SerializeField] private int _maxEnemies;

    [SerializeField] private int _maxEliteEnemies;


    public List<SpawnableData> Enemies => _enemies;

    public List<SpawnableData> EliteEnemies => _eliteEnemies;

    public List<SpawnableData> Bonuses => _bonuses;

    public int SpawnDelay => (int)(_spawnDelay * GlobalConstants.MillisecondsConverter);

    public int SpawnTime => (int)(_spawnTime * GlobalConstants.MillisecondsConverter);

    public int EliteSpawnTime => (int)(_eliteSpawnTime * GlobalConstants.MillisecondsConverter);

    public int BonusSpawnTime => (int)(_bonusSpawnTime * GlobalConstants.MillisecondsConverter);

    public int MaxEnemies => _maxEnemies;

    public int MaxHardEnemies => _maxEliteEnemies;
}