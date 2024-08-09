using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    private static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();

    private GameObject _objectPoolEmptyHolder;

    private static GameObject _ParticleSystemEmpty;
    private static GameObject _enemyEmpty;
    private static GameObject _weaponEmpty;
    private static GameObject _bonusEmpty;

    public enum PoolType
    {
        ParticleSystem,
        Enemy,
        Weapon,
        Bonus,
        None
    }

    public static PoolType PoolingType;

    private void Awake()
    {
        SetupEmpties();
    }

    private void SetupEmpties()
    {
        _objectPoolEmptyHolder = new GameObject("Pooled Objects");
        _objectPoolEmptyHolder.transform.SetParent(transform);

        _ParticleSystemEmpty = new GameObject("Particle Effects");
        _ParticleSystemEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);

        _enemyEmpty = new GameObject("Enemies");
        _enemyEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);

        _weaponEmpty = new GameObject("Weapons");
        _weaponEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);

        _bonusEmpty = new GameObject("Bonuses");
        _bonusEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.None)
    {
        PooledObjectInfo pool = ObjectPools.Find(objectInfo => objectInfo.LookupString == objectToSpawn.name);

        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        GameObject spawnableObject = pool.InnactiveObjects.FirstOrDefault();

        if (spawnableObject == null)
        {
            GameObject parrentObject = SetParrentObject(poolType);

            spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);

            if (parrentObject != null)
            {
                spawnableObject.transform.SetParent(parrentObject.transform);
            }
        }

        else
        {
            spawnableObject.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            pool.InnactiveObjects.Remove(spawnableObject);
            spawnableObject.SetActive(true);
        }

        return spawnableObject;
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Transform parrentTransform)
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);

        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        GameObject spawnableObject = pool.InnactiveObjects.FirstOrDefault();

        if (spawnableObject == null)
        {
            spawnableObject = Instantiate(objectToSpawn, parrentTransform);
        }

        else
        {
            pool.InnactiveObjects.Remove(spawnableObject);
            spawnableObject.SetActive(true);
        }

        return spawnableObject;
    }

    //функция отключения вместо Destroy();
    public static void ReturnObjectToPool(GameObject objectToReturn)
    {
        string objectName = objectToReturn.name.Substring(0, objectToReturn.name.Length - 7); // удаляем (Clone) из имени

        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectName);

        if (pool == null)
        {
            Debug.LogWarning("Invalid function for an object " + objectToReturn.name);
        }

        else
        {
            objectToReturn.SetActive(false);
            pool.InnactiveObjects.Add(objectToReturn);
        }
    }

    private static GameObject SetParrentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.ParticleSystem:
                return _ParticleSystemEmpty;

            case PoolType.Enemy:
                return _enemyEmpty;

            case PoolType.Weapon:
                return _weaponEmpty;

            case PoolType.Bonus:
                return _bonusEmpty;

            case PoolType.None:
                return null;

            default:
                return null;
        }
    }
}


public class PooledObjectInfo
{
    public string LookupString;

    public List<GameObject> InnactiveObjects = new List<GameObject>();
}
