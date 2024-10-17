using UnityEngine;

public abstract class BossStageData : ScriptableObject
{
    [SerializeField] private GameObject _projectile;

    public GameObject Projectile => _projectile;

    public abstract BossStage GetBossStage();
}