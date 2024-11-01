using System.Collections.Generic;
using UnityEngine;

public enum Abilities
{
    Shield,
    MissileLaunch,
    Multilaser
}

[CreateAssetMenu(fileName = "InterceptorCasterStage", menuName = "Config Data/Spawnable Data/Boss/BossStages/InterceptorCasterStage")]
public class InterceptorWithAbilitiesStageData : InterceptorStageData, IAbilityCasterData
{
    [SerializeField] protected float _cooldown;
    [SerializeField] protected Abilities[] _abilities;
    public float Cooldown => _cooldown;

    public override BossStage GetBossStage() => new InterceptorWithAbilities(this);
  
    public List<IBossAbility> GetAbilities()
    {
        List<IBossAbility> result = new List<IBossAbility>(_abilities.Length);

        foreach (var ability in _abilities)
        {
            switch (ability)
            {
                case Abilities.Shield:
                    result.Add(new BossShieldAbility());
                    break;

                case Abilities.MissileLaunch:
                    result.Add(new BossMissileLaunchAbility());
                    break;
            }
        }
        return result;
    }
}