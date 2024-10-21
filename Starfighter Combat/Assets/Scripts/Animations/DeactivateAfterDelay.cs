using Cysharp.Threading.Tasks;
//using System.Collections;
using UnityEngine;

public class DeactivateAfterDelay : MonoBehaviour
{
    [SerializeField] private float _destroyTimer = 1f;
    private int _millisecondsDelay;

    //private WaitForSeconds _delay;

    private void Awake() => _millisecondsDelay = (int)(_destroyTimer * GlobalConstants.MillisecondsConverter);

    private void OnEnable() => Deactivate().Forget();
    //{
    //    //StartCoroutine(DestroyTimer());
    //    Destroy(gameObject, _destroyTimer);
    //}

    private async UniTaskVoid Deactivate()
    {
        await UniTask.Delay(_millisecondsDelay, cancellationToken: destroyCancellationToken);

        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    //private IEnumerator DestroyTimer()
    //{
    //    yield return new WaitForSeconds(_destroyTimer);

    //    ObjectPoolManager.ReturnObjectToPool(gameObject);

    //}

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