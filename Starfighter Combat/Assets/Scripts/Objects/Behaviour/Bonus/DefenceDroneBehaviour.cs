using UnityEngine;

public class DefenceDroneBehaviour : MonoBehaviour
{
    [SerializeField] PlayerMissile _missile;
    private IAttacker _droneAttackHandler;

    private void Awake()
    {
        
        _droneAttackHandler = new Attacker(this);
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        _droneAttackHandler.Fire(_missile.gameObject);
    }

    private void OnDisable()
    {
        _droneAttackHandler.Reset();
    }
}