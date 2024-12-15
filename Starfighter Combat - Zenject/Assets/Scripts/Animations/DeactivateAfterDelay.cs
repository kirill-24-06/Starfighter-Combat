using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class DeactivateAfterDelay : MonoBehaviour
{
    [SerializeField] private float _destroyTimer = 1f;
    private int _millisecondsDelay;

    private CancellationToken _token;

    private void Awake()
    {
        _millisecondsDelay = (int)(_destroyTimer * GlobalConstants.MillisecondsConverter);
        _token = EntryPoint.Instance.destroyCancellationToken;

        PoolRootMap.SetParrentObject(gameObject,GlobalConstants.PoolTypesByTag[ObjectTag.Particles]);
    }
   
    private void OnEnable() => Deactivate().Forget();
    
    private async UniTaskVoid Deactivate()
    {
        await UniTask.Delay(_millisecondsDelay, cancellationToken: _token);

        ObjectPool.Release(gameObject);
    }
}