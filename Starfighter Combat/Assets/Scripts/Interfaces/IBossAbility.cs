using UnityEngine;

public interface IBossAbility
{
    public abstract void Initialise(Boss boss);
    public abstract void Cast();
}

public class BossMultilaserGun : IBossAbility
{
    private EnemyAdvancedAttacker _multilaserGun;
    private GameObject _projectile;

    public void Initialise(Boss boss)
    {
        //_multilaserGun = new EnemyAdvancedAttacker(boss, boss.CurrentStage.StageData, 10);
        _projectile = (GameObject)Resources.Load("Prefabs/Weapon/EnemyLaser");
    }

    public void Cast()
      {
        _multilaserGun.Fire(_projectile);
    }
}

public class BossSummonMinionsAbility : IBossAbility
{
    private Spawner _spawner;
    private SpawnableData _minion;

    public void Initialise(Boss boss)
    {
        
    }

    public void Cast()
    {
       _spawner.SpawnEnemy(_minion);
    }
}
