using UnityEngine;

public class DefenceDroneBehaviour : MonoBehaviour
{
    [SerializeField] GameObject _explosionPrefab;
    [SerializeField] DefenceDroneData _data;

    private IAttacker _droneAttackHandler;
    private IResetable _attacker;

    public GameObject Explosion => _explosionPrefab;

    private void Start()
    {
        var attacker = new EnemyAttacker(this, _data);
        _attacker = attacker;
        _droneAttackHandler = attacker;

        gameObject.SetActive(false);
    }

    private void Update() => _droneAttackHandler.Fire();

    private void OnDisable() => _attacker.Reset(); 
}
