using Utils.Events.Channel.Static;
using Zenject;

namespace Refactoring
{
    public class TimeBar : Bar
    {
        private Timer _timer;

        [Inject]
        public void Construct([Inject(Id = "BonusTimer")] Timer timer)
        {
            _timer = timer;
        }

        private void Awake()
        {
            Channel<StopEvent>.OnEvent += OnGameStop;
            Channel<PlayerRespawnEvent>.OnEvent += OnPlayerRespawn;
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

        private void OnPlayerRespawn(PlayerRespawnEvent @event) => gameObject.SetActive(false);

        private void OnTimerEnd() => gameObject.SetActive(false);

        private void OnGameStop(StopEvent @event) => gameObject.SetActive(false);

        private void OnDestroy()
        {
            Channel<StopEvent>.OnEvent -= OnGameStop;
            Channel<PlayerRespawnEvent>.OnEvent -= OnPlayerRespawn;
        }
    }

}
