public class BossShieldAbility : IBossAbility
{
    private BossShieldBehaviour _bossShield;

    public void Initialise(Boss boss)
    {
        _bossShield = boss.transform.Find("BossShield").gameObject.GetComponent<BossShieldBehaviour>();
    }

    public void Cast()
    {
        if (_bossShield.IsActive)
            _bossShield.RenewShield();

        else
            _bossShield.gameObject.SetActive(true);
    }
}
