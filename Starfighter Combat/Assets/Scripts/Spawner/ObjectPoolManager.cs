using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();

    private GameObject _objectPoolEmptyHolder;

    private static GameObject _ParticleSystemEmpty;
    private static GameObject _gameObjectsEmpty;

    public enum PoolType
    {
        ParticleSystem,
        GameObject,
        None
    }

    public static PoolType PoolingType;

    private void Awake()
    {
        //группируем пуллы обьектов в иерархии по 
        SetupEmpties();
    }

    private void SetupEmpties()
    {
        _objectPoolEmptyHolder = new GameObject("Pooled Objects");
        _objectPoolEmptyHolder.transform.SetParent(transform);

        _ParticleSystemEmpty = new GameObject("Particle Effects");
        _ParticleSystemEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);

        _gameObjectsEmpty = new GameObject("Game Objects");
        _gameObjectsEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
    }

    // сама функция спавна вместо Instantiate();
    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation , PoolType poolType = PoolType.None)
    {
        //Сначала ищем пулл в котором должен содержаться обьект

        PooledObjectInfo pool = ObjectPools.Find(objectInfo => objectInfo.LookupString == objectToSpawn.name);

        //Если пулла обьектов нет(т.е обьект ни разу не спавнился)
        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        //Теперь в пулле неактивных обьектов ищем кандидата на спавн

        GameObject spawnableObject = pool.InnactiveObjects.FirstOrDefault();

        // если такого обьекта нет создаём новый

        if (spawnableObject == null)
        {
            //засовываем в родительскую папку в иерархии
            GameObject parrentObject = SetParrentObject(poolType);

            spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);

            if(parrentObject != null)
            {
                spawnableObject.transform.SetParent(parrentObject.transform);
            }
        }
        //иначе активируем неактивный обьект если он есть в пулле
        else
        {
            spawnableObject.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            pool.InnactiveObjects.Remove(spawnableObject);
            spawnableObject.SetActive(true);
        }

        return spawnableObject;
    }

    //аналог для спавна на конкретной точке (как в первом прототипе)
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
            //Обьект НЕ был создан при помощи ObjectPoolManager и НЕ должен использовать эту функцию
            Debug.LogWarning("Недопустимая функция для обьекта " + objectToReturn.name);
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

            case PoolType.GameObject:
                return _gameObjectsEmpty;

            case PoolType.None:
                return null;

            default:
                return null;
        }
    }
}

//отдельный пулл для каждого префаба
public class PooledObjectInfo
{
    public string LookupString;

    public List<GameObject> InnactiveObjects = new List<GameObject>();
}
