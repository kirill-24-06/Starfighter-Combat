using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private static Dictionary<string, Stack<GameObject>> _poolMap;
    private static readonly string _delete = "(Clone)";

    private void Awake()
    {
        _poolMap = new Dictionary<string, Stack<GameObject>>();
    }

    public static Stack<GameObject> NewPool(GameObject objectToPrewarm, int prewarmAmount = 3)
    {
        var pool = new Stack<GameObject>();
        var key = objectToPrewarm.name.Replace(_delete, string.Empty);
        _poolMap.Add(key, pool);

        for (int i = 0; i < prewarmAmount; i++)
        {
            var obj = Instantiate(objectToPrewarm);
            Release(obj);
        }

        return pool;
    }

    public static GameObject Get(GameObject objectToGet, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        var key = objectToGet.name;
        _poolMap.TryGetValue(key, out var pool);

        pool ??= NewPool(objectToGet);

        var obj = pool.Count > 0 ? pool.Pop() : Instantiate(objectToGet);

        obj.transform.SetPositionAndRotation(spawnPosition, spawnRotation); //????
        obj.SetActive(true);
        return obj;
    }

    public static void Release(GameObject obj)
    {
        string key = obj.name.Replace(_delete, string.Empty);

        _poolMap.TryGetValue(key, out var pool);

        pool ??= NewPool(obj, 0);

        obj.SetActive(false); //???
        pool.Push(obj);
    }

    private void OnDestroy() => _poolMap.Clear();
}