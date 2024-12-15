using Zenject;

public class ForceFieldBonus : IBonus
{
    private HUDManager _hud;
    private Timer _timer;
    private float _bonusLenght;

    private EventManager _events;

    public ForceFieldBonus(HUDManager hud, [Inject(Id = "BonusTimer")] Timer timer, float bonusLenght, EventManager events)
    {
        _hud = hud;
        _timer = timer;
        _bonusLenght = bonusLenght;
        _events = events;
    }

    public void Handle() => ActivateForceField();

    private void ActivateForceField()
    {
        _events.ForceField?.Invoke(true);

        _timer.SetTimer(_bonusLenght);
        _timer.TimeIsOver += DeactivateForceField;
        _timer.StartTimer();

       _hud.ActivateBonusTimer();
    }

    private void DeactivateForceField()
    {
        _events.ForceField?.Invoke(false);
        _timer.ResetTimer();
    }
}

