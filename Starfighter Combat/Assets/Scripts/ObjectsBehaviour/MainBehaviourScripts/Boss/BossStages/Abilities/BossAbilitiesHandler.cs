using System.Collections.Generic;
using UnityEngine;

public class BossAbilitiesHandler
{
    private List<IBossAbility> _abilities;
    private Timer _timer;

    private float _cooldown;
    private bool _onCooldown = false;

    public BossAbilitiesHandler(Boss boss, IAbilityCasterData abilityCasterData)
    {
        _cooldown = abilityCasterData.Cooldown;

        _timer = new Timer(boss);
        _timer.TimeIsOver += () => _onCooldown = false;

        _abilities = abilityCasterData.GetAbilities();

        foreach (var ability in _abilities)
            ability.Initialise(boss);
    }

    public void CastAbility()
    {
        if (_onCooldown)
            return;

        int abilityIndex = Random.Range(0, _abilities.Count);

        _abilities[abilityIndex].Cast();
        _onCooldown = true;

        _timer.SetTimer(_cooldown);
        _timer.StartTimer();
    }
}