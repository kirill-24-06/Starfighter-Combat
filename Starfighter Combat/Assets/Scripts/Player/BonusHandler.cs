public class BonusHandler
{
    private BonusHandler() 
    {
        _events = EventManager.GetInstance();
    }

    private static BonusHandler _instance;
    private EventManager _events;

    public static BonusHandler GetInstance()
    {
        _instance ??= new BonusHandler();

        return _instance;
    }

    public void ActivateMultilaser(Timer timer, float bonusTimeLenght)
    {
        _events.Multilaser?.Invoke();

        timer.SetTimer(bonusTimeLenght);
        timer.TimeIsOver += _events.MultilaserEnd;
        timer.StartTimer();
    }

    public void ActivateLaserBeam(Timer timer, float bonusTimeLenght)
    {
        _events.Beam?.Invoke();

        timer.SetTimer(bonusTimeLenght);
        timer.TimeIsOver += _events.BeamEnd;
        timer.StartTimer();
    }

    public void ActivateForceField(Timer timer, float bonusTimeLenght)
    {
       
        _events.ForceField?.Invoke();

        timer.SetTimer(bonusTimeLenght);
        timer.TimeIsOver += _events.ForceFieldEnd;
        timer.StartTimer();
    }

    public void ActivateDrone(PlayerBehaviour player)
    {  
        _events.Drone?.Invoke();
    }
}