
public class DefenceDroneBehaviour : ObjectBehaviour
{
    private IAttacker _droneAttackHandler;

    private void Awake()
    {
        _objectMoveHandler = null;
        _droneAttackHandler = new Attacker(this);
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        _droneAttackHandler.Fire(ObjectInfo.Projectile);
    }

    private void OnDisable()
    {
        _droneAttackHandler.Reset();
    }
}