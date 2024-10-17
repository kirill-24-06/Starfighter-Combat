using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : MonoBehaviour
{
    [SerializeField] protected BossData _data;
    protected EventManager _events;
    protected int _currentHealth;

    private List<BossStage> _stages;
    private int _currentStage;

    protected AudioSource _audioPlayer;

    protected bool _isInvunerable = false;

    public BossData Data => _data;
    public int CurrentHealth => _currentHealth;

    public BossStage CurrentStage => _stages[_currentStage];

    protected abstract void Disable();

    private void Awake()
    {
        _events = EntryPoint.Instance.Events;
        Initialise();
    }

    protected virtual void OnEnable()
    {
        _events.IonSphereUse += OnIonSphereUse;
        _events.EnemyDamaged += OnDamaged;
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

        _audioPlayer = EntryPoint.Instance.GlobalSoundFX;
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

    protected virtual void TakeDamage(int damage)
    {
        if (_isInvunerable || damage <= 0)
            return;

        _currentHealth -= damage;

        if (_currentHealth < 0) _currentHealth = 0;

        if (_currentHealth == 0) Disable();
    }

    public void SetInvunrability(bool value) => _isInvunerable = value;

    protected virtual void OnIonSphereUse()
    {
        if (gameObject.activeInHierarchy)
            TakeDamage(5);
    }

    protected virtual void OnDisable()
    {
        _events.IonSphereUse -= OnIonSphereUse;
        _events.EnemyDamaged -= OnDamaged;
    }

    protected void OnDamaged(GameObject enemy, int damage)
    {
        if (enemy == gameObject)
            TakeDamage(damage);
    }
}