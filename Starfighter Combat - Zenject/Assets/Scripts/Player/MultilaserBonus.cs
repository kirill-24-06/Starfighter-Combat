using Zenject;

public class MultilaserBonus : IBonus
{
    private HUDManager _hud;
    private Timer _timer;
    private float _bonusLenght;

    private EventManager _events;

   public MultilaserBonus(HUDManager hud,[Inject(Id ="BonusTimer")] Timer timer, float bonusLenght, EventManager events)
    {
        _hud = hud;
        _timer = timer;
        _bonusLenght = bonusLenght;
        _events = events;
    }

    public void Handle() => EnableMultilaser();

    private void EnableMultilaser()
    {
        _events.Multilaser?.Invoke(true);

        _timer.SetTimer(_bonusLenght);
        _timer.TimeIsOver += OnMultilaserEnd;
        _timer.StartTimer();

        _hud.ActivateBonusTimer();
    }

    private void OnMultilaserEnd()
    {
        _events.Multilaser?.Invoke(false);
        _timer.ResetTimer();
    }
}
