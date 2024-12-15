public class RepairBonus : IBonus
{
    private int _healthAmount;
    private EventManager _events;

    public RepairBonus(int healthAmount, EventManager events)
    {
        _healthAmount = healthAmount;
        _events = events;
    }

    public void Handle() => _events.PlayerHealed?.Invoke(_healthAmount);
}
