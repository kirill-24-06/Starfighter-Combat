using UnityEngine;

public class BossShieldBehaviour : MonoBehaviour, IInteractableEnemy
{
    [SerializeField] private PolygonCollider2D _bossCollider;
    [SerializeField] private int _maxHealth = 15;
    public int _currentHealth;

    public bool IsActive { get; private set; } = false;

    private void Start()
    {
        EntryPoint.Instance.CollisionMap.Register(GetComponent<Collider2D>(), this);
    }

    private void OnEnable()
    {
        _currentHealth = _maxHealth;
        IsActive = true;
        _bossCollider.enabled = false;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (Player.IsPlayer(collision.gameObject) || collision.gameObject == EntryPoint.Instance.Player.ForceField.gameObject)
            Collide();
    }

    private void Collide()
    {
        if (!EntryPoint.Instance.Player.IsInvunerable && !EntryPoint.Instance.Player.IsDroneActive)
            EntryPoint.Instance.Events.PlayerDamaged?.Invoke(GlobalConstants.CollisionDamage);

        else if (!EntryPoint.Instance.Player.IsInvunerable && EntryPoint.Instance.Player.IsDroneActive)
            EntryPoint.Instance.Events.DroneDestroyed?.Invoke();

        TakeDamage(GlobalConstants.CollisionDamage);
    }

    public void Interact() => TakeDamage(GlobalConstants.CollisionDamage);

    public void TakeDamage(int damage)
    {

        _currentHealth -= damage;

        if (_currentHealth < 0)
            _currentHealth = 0;

        if (_currentHealth == 0)
            gameObject.SetActive(false);
    }

    public void RenewShield()
    {
        _currentHealth = _maxHealth;
    }

    private void OnDisable()
    {
        IsActive = false;
        _bossCollider.enabled = true;
    }
}
