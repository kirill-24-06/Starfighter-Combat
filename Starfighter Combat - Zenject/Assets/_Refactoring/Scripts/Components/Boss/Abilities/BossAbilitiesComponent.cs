using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Refactoring
{
    public class BossAbilitiesComponent
    {
        private List<IBossAbility> _abilities;

        public BossAbilitiesComponent(List<IBossAbility> bossAbilities)
        {
            _abilities = bossAbilities;
        }

        public void UseAbility()
        {
            int abilityIndex = Random.Range(0, _abilities.Count);

            _abilities[abilityIndex].Cast();
        }
    }
}