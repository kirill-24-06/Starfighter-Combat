
public class TimeBar : Bar
{
    private Timer _timer;

    public void Initialise(Timer timer)
    {
        _timer = timer;
        gameObject.SetActive(false);

        EventManager.GetInstance().Stop += OnGameStop;
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

    private void OnTimerEnd()
    {
        gameObject.SetActive(false);
    }

    private void OnGameStop()
    {
        gameObject.SetActive(false);
    }
}
