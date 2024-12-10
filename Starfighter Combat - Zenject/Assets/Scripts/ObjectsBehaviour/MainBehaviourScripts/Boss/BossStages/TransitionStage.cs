using UnityEngine;

public class TransitionStage : BossStage
{
    private TransitionStageData _data;
    private BossDefenceDrone[] _defenceDrones;
    private Spawner _spawner;

    private Animator[] _engines;

    private GameObject _emergencyShield;

    private int _activeDronesCount;
    private int _activeDrones;
    private bool _alreadyStarted = false;

    public TransitionStage(TransitionStageData data)
    {
        _data = data;
    }

    public override BossStage Initialise(Boss boss)
    {
        _boss = boss;

        _emergencyShield = _boss.transform.Find("EmergencyShield").gameObject;
        _defenceDrones = new BossDefenceDrone[_data.DefenceDronesAmount];
        _activeDrones = _data.DefenceDronesAmount;

        _engines = _boss.GetComponentsInChildren<Animator>();

        _spawner = EntryPoint.Instance.Spawner;

        return this;
    }
    public override void Handle()
    {
        OnStageStart();
        Attack();
    }

    public override bool CheckCompletion()
    {
        if (_activeDrones > 0)
            return false;

        else
        {           
            _emergencyShield.SetActive(false);
            _boss.SetInvunrability(false);

            foreach (var engine in _engines)
            {
                engine.gameObject.SetActive(true);
            }

            return true;
        }
    }

    private void OnStageStart()
    {
        if (_alreadyStarted) return;

        _alreadyStarted = true;

        _emergencyShield.SetActive(true);
        _boss.SetInvunrability(true);

        foreach (var engine in _engines)
        {
            engine.gameObject.SetActive(false);
        }

        for (int i = 0; i < _defenceDrones.Length; i++)
        {
            _defenceDrones[i] = _spawner.SpawnEnemy(_data.BossDefenceDrone).GetComponent<BossDefenceDrone>().InitialiseByBoss();
        }
    }

    private void Attack()
    {
        foreach (var drone in _defenceDrones)
        {
            if (drone.gameObject.activeInHierarchy)
            {
                _activeDronesCount++;
                drone.Handle();
            }
        }

        _activeDrones = _activeDronesCount;
        _activeDronesCount = 0;
    }
}
