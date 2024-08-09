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

    private ObjectHolder() { }

    public static ObjectHolder GetInstance()
    {
        //if (_instance == null)
        //{
        //    _instance = new ObjectHolder();
        //}

        //ÈËÈ
        _instance ??= new ObjectHolder();

        return _instance;
    }

    public void RegisterObject(GameObject objectToRegistr, ObjectTag tag = ObjectTag.None)
    {
        HoldedOojectInfo list = _createdObjectsLists.Find(objectInfo => objectInfo.LookupTag == tag);

        if (list == null)
        {
            list = new HoldedOojectInfo() { LookupTag = tag };
            _createdObjectsLists.Add(list);
        }

        list.RegisteredObjects.Add(objectToRegistr);
    }

    public bool FindRegisteredObject(GameObject objectToFind, ObjectTag objectTag = ObjectTag.None)
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
}

public class HoldedOojectInfo
{
    public ObjectTag LookupTag;

    public List<GameObject> RegisteredObjects = new List<GameObject>();
}
