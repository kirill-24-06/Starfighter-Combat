using System.Collections;
using UnityEngine;

public class DeactivateAfterDelay : MonoBehaviour
{
    [SerializeField] private float _destroyTimer = 1f;

    private void OnEnable()
    {
        //StartCoroutine(DestroyTimer());
        Destroy(gameObject, _destroyTimer);
    }

    //private IEnumerator DestroyTimer()
    //{
    //    float elapsedTime = 0;

    //    while (elapsedTime < _destroyTimer)
    //    {
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }
    //    ObjectPoolManager.ReturnObjectToPool(gameObject);
    //}
}