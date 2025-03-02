using UnityEngine;
using Utils.SpawnSystem;
using System.Collections.Generic;
using Random = UnityEngine.Random;


namespace Refactoring
{
    public class MinionsSpawner
    {
        private Dictionary<MonoProduct, IFactory<MonoProduct>> _factories;

        private SpawnArea[] _spawnAreas;

        public MinionsSpawner(
            SpawnArea[] spawnAreas,
            List<MonoProduct> prefabs,
            List<IFactory<MonoProduct>> factories)
        {
            _factories = new Dictionary<MonoProduct, IFactory<MonoProduct>>();

            for (int i = 0; i < prefabs.Count; i++)
            {
                _factories.Add(prefabs[i], factories[i]);
            }

            _spawnAreas = spawnAreas;

            for (int i = 0; i < _spawnAreas.Length; i++)
                _spawnAreas[i].Initialise(_spawnAreas[i].name);
        }

        public GameObject SpawnUnit(IEnemySpawnSettings settings)
        {
            var area = SelectSpawnArea(settings.SpawnZones);

            var rotation = GetPrefabRotation(settings, area);

            var enemy = _factories[settings.UnitToSpawn].Create();

            enemy.transform.SetLocalPositionAndRotation(area.GenerateSpawnPosition(), rotation);

            return enemy.gameObject;
        }

        private Quaternion GetPrefabRotation(IEnemySpawnSettings settings, SpawnArea area)
        {
            return !settings.UsePrefabRotation ? area.Rotation : settings.UnitToSpawn.transform.rotation;
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
}
