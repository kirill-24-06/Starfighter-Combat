
public class TimeBar : Bar
{
    private Timer _timer;

    public void Initialise(Timer timer)
    {
        _timer = timer;
        gameObject.SetActive(false);

        EntryPoint.Instance.Events.Stop += OnGameStop;
        EntryPoint.Instance.Events.PlayerRespawn += OnPlayerRespawn;
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
        EntryPoint.Instance.Events.Stop -= OnGameStop;
        EntryPoint.Instance.Events.PlayerRespawn -= OnPlayerRespawn;
    }
}
