using Cysharp.Threading.Tasks;
using UnityEngine;

public class DeactivateAfterDelay : MonoBehaviour
{
    [SerializeField] private float _destroyTimer = 1f;
    private int _millisecondsDelay;

    private void Awake() => _millisecondsDelay = (int)(_destroyTimer * GlobalConstants.MillisecondsConverter);

    private void OnEnable() => Deactivate().Forget();
    
    private async UniTaskVoid Deactivate()
    {
        await UniTask.Delay(_millisecondsDelay, cancellationToken: destroyCancellationToken);

        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}