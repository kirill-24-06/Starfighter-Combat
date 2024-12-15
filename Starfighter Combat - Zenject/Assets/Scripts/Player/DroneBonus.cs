
public class DroneBonus : IBonus
{
    private EventManager _events;

    public DroneBonus(EventManager events)
    {
        _events = events;
    }

    public void Handle() => _events.AddDrone?.Invoke();

}