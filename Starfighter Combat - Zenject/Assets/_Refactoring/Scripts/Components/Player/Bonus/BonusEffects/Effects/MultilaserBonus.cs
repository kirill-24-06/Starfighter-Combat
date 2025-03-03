using Utils.Events.Channel.Static;
using Zenject;

namespace Refactoring
{
    public class MultilaserBonus : IBonus
    {
        private HUDManager _hud;
        private Timer _timer;
        private float _bonusLenght;

        private MultilaserEvent _enableMultilaser;

        public MultilaserBonus(HUDManager hud, [Inject(Id = "BonusTimer")] Timer timer, float bonusLenght)
        {
            _hud = hud;
            _timer = timer;
            _bonusLenght = bonusLenght;
        }

        public void Handle() => EnableMultilaser();

        private void EnableMultilaser()
        {
            Channel<MultilaserEvent>.Raise(_enableMultilaser.SetBool(true));

            _timer.SetTimer(_bonusLenght);
            _timer.TimeIsOver += OnMultilaserEnd;
            _timer.StartTimer();

            _hud.ActivateBonusTimer();
        }

        private void OnMultilaserEnd()
        {
            Channel<MultilaserEvent>.Raise(_enableMultilaser.SetBool(false));
            _timer.ResetTimer();
        }
    }

}



