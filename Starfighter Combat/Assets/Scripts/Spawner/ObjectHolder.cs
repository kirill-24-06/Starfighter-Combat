using System.Collections.Generic;
using UnityEngine;

public enum ObjectTag
{
    Enemy,
    Player,
    Bonus,
    Boss,
    PlayerWeapon,
    EnemyWeapon,
    None
}

//all created objects are recorded here by tags
//Unlike the Object Pool, all created objects are stored here
public class ObjectHolder
{
    private List<HoldedOojectInfo> _createdObjectsLists = new List<HoldedOojectInfo>();

    private static ObjectHolder _instance;

    private ObjectHolder()
    {   
        EventManager.GetInstance().Stop += OnGameOver;
    }

    public static ObjectHolder GetInstance()
    {
        //ИЛИ
        _instance ??= new ObjectHolder();

        return _instance;
    }

    public void RegisterObject(GameObject objectToRegistr, ObjectTag tag)
    {
        bool isAlreadyRegistered = false;

        HoldedOojectInfo list = _createdObjectsLists.Find(objectInfo => objectInfo.LookupTag == tag);

        if (list == null)
        {
            list = new HoldedOojectInfo() { LookupTag = tag };
            _createdObjectsLists.Add(list);

            list.RegisteredObjects.Add(objectToRegistr);
        }
        else
        {
            foreach (var registeredObject in list.RegisteredObjects)
            {
                if (objectToRegistr == registeredObject)
                {
                    isAlreadyRegistered = true;
                    break;
                }
            }

            if (!isAlreadyRegistered)
            {
                list.RegisteredObjects.Add(objectToRegistr);
            }
        }
    }

    public bool FindRegisteredObject(GameObject objectToFind, ObjectTag objectTag)
    {
        HoldedOojectInfo list = _createdObjectsLists.Find(objectInfo => objectInfo.LookupTag == objectTag);

        if (list == null)
        {
            return false;
        }

        else
        {
            foreach (var registeredObject in list.RegisteredObjects)
            {
                if (objectToFind == registeredObject)
                    return true;
            }

            return false;
        }
    }

    public List<GameObject> GetRegisteredObjectsByTag(ObjectTag tag)
    {
        List<GameObject> result = new List<GameObject>();

        HoldedOojectInfo list = _createdObjectsLists.Find(objectInfo => objectInfo.LookupTag == tag);

        if (list != null)
        {
            foreach (var item in list.RegisteredObjects)
            {
                result.Add(item);
            }
        }
        else
        {
            Debug.LogWarning("Таких объектов нет!!!");
        }

        return result;
    }

    public static void OnGameOver()
    {
        _instance?.Clear();
        EventManager.GetInstance().PlayerDied -= OnGameOver;
    }

    private void Clear()
    {
        foreach (var item in _createdObjectsLists)
        {
            item.RegisteredObjects.Clear();
        }

        _createdObjectsLists.Clear();
        _instance = null;
    }
}

public class HoldedOojectInfo
{
    public ObjectTag LookupTag;

    public List<GameObject> RegisteredObjects = new List<GameObject>();
}
