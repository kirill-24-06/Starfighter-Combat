using Zenject;

public class TimeBar : Bar
{
    private Timer _timer;
    private EventManager _events;

    [Inject]
    public void Construct([Inject(Id ="BonusTimer")]Timer timer, EventManager events)
    {
        _timer = timer;
        _events = events;

        _events.Stop += OnGameStop;
        _events.PlayerRespawn += OnPlayerRespawn;
    }

    private void OnEnable()
    {
        if (_timer != null)
        {
            _timer.HasBeenUpdated += OnValueChange;
            _timer.TimeIsOver += OnTimerEnd;
        }
    }

    private void OnDisable()
    {
        if (_timer != null)
        {
            _timer.HasBeenUpdated -= OnValueChange;
            _timer.TimeIsOver -= OnTimerEnd;
        }
    }

    private void OnPlayerRespawn() => gameObject.SetActive(false);

    private void OnTimerEnd() => gameObject.SetActive(false);

    private void OnGameStop() => gameObject.SetActive(false);

    private void OnDestroy()
    {
        _events.Stop -= OnGameStop;
        _events.PlayerRespawn -= OnPlayerRespawn;
    }
}
