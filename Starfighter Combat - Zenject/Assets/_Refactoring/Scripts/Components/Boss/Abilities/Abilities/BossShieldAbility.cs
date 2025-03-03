
namespace Refactoring
{
    public class BossShieldAbility : IBossAbility
    {
        private BossShield _bossShield;

        public BossShieldAbility(BossShield bossShield)
        {
           _bossShield = bossShield;
        }

        public void Cast()
        {
            if (_bossShield.IsActive)
                _bossShield.RenewShield();

            else
                _bossShield.gameObject.SetActive(true);
        }
    }
}