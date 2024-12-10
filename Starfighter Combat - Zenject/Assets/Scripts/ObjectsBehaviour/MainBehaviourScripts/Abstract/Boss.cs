using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : MonoBehaviour, IInteractableEnemy, INukeInteractable
{
    [SerializeField] protected BossData _data;
    protected GameObject _gameObject;
    protected Transform _transform;

    private List<BossStage> _stages;
    private int _currentStage;
    protected int _currentHealth;

    protected IDamageble _damageHandler;
    protected IResetable _health;

    protected AudioSource _audioPlayer;
    protected EventManager _events;

    public BossData Data => _data;
    public int CurrentHealth => _currentHealth;

    protected bool _isInvunerable = false;
    protected bool _isInPool = true;

    protected abstract void OnDeath();

    protected virtual void Awake()
    {
        _events = EntryPoint.Instance.Events;
        _audioPlayer = EntryPoint.Instance.GlobalSoundFX;
        _gameObject = gameObject;
        _transform = gameObject.transform;
    }

    protected virtual void Initialise()
    {
        _currentHealth = _data.MaxHealth;
        _currentStage = 0;

        _stages = new List<BossStage>(_data.Stages.Length);

        for (int i = 0; i < _data.Stages.Length; i++)
        {
            _stages.Add(_data.Stages[i].GetBossStage().Initialise(this));
        }
    }

    private void Update()
    {
        _stages[_currentStage].Handle();
        CheckStageCompleted();
    }

    private void CheckStageCompleted()
    {
        if (_stages[_currentStage].CheckCompletion())
        {
            SwitchStage();
        }
    }

    private void SwitchStage()
    {
        if (_currentStage + 1 < _stages.Count)
        {
            _currentStage++;
        }
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (Player.IsPlayer(collision.gameObject) || collision.gameObject == EntryPoint.Instance.Player.ForceField.gameObject)
            Collide();
    }

    private void Collide()
    {
        if (!EntryPoint.Instance.Player.IsInvunerable && !EntryPoint.Instance.Player.IsDroneActive)
            _events.PlayerDamaged?.Invoke(GlobalConstants.CollisionDamage);

        else if (!EntryPoint.Instance.Player.IsInvunerable && EntryPoint.Instance.Player.IsDroneActive)
            _events.DroneDestroyed?.Invoke();

        TakeDamage(GlobalConstants.CollisionDamage);
    }

    public void Interact() => TakeDamage(GlobalConstants.CollisionDamage);

    protected virtual void TakeDamage(int damage)
    {
        if (_isInvunerable)
            return;

        _damageHandler.TakeDamage(damage);
    }

    public void SetInvunrability(bool value) => _isInvunerable = value;

    public void GetDamagedByNuke() => TakeDamage(5);
}