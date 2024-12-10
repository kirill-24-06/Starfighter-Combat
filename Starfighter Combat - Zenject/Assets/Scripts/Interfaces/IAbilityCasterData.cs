using System.Collections.Generic;

public interface IAbilityCasterData
{
    public float Cooldown { get; }

    public List<IBossAbility> GetAbilities();
}
