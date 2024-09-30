using UnityEngine;

public class DefenceDroneBehaviour : MonoBehaviour
{
    [SerializeField] PlayerMissile _missile;
    [SerializeField] GameObject _explosionPrefab;
    private IAttacker _droneAttackHandler;

    public GameObject Explosion => _explosionPrefab;

    private void Awake() => _droneAttackHandler = new Attacker(this);

    private void Start() => gameObject.SetActive(false);

    private void Update() => _droneAttackHandler.Fire(_missile.gameObject);

    private void OnDisable()
    {
        _droneAttackHandler.Reset();
    }
}