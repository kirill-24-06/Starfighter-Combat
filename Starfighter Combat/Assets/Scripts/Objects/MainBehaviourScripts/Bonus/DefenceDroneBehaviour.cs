using UnityEngine;

public class DefenceDroneBehaviour : MonoBehaviour
{
    [SerializeField] PlayerMissile _missile;
    [SerializeField] GameObject _explosionPrefab;
    [SerializeField] DefenceDroneData _data;


    private IAttacker _droneAttackHandler;

    public GameObject Explosion => _explosionPrefab;

    private void Start()
    {
        _droneAttackHandler = new EnemyAttacker(this, _data);
        gameObject.SetActive(false);
    }

    private void Update() => _droneAttackHandler.Fire(_missile.gameObject);

    private void OnDisable()
    {
        _droneAttackHandler.Reset();
    }
}
