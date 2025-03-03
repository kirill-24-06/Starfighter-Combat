using Utils.Events.Channel.Static;
using Zenject;

namespace Refactoring
{
    public class ForceFieldBonus : IBonus
    {
        private HUDManager _hud;
        private Timer _timer;
        private float _bonusLenght;

        private ForceFieldEvent _forceFieldActive;

        public ForceFieldBonus(HUDManager hud, [Inject(Id = "BonusTimer")] Timer timer, float bonusLenght)
        {
            _hud = hud;
            _timer = timer;
            _bonusLenght = bonusLenght;
        }

        public void Handle() => ActivateForceField();

        private void ActivateForceField()
        {
            Channel<ForceFieldEvent>.Raise(_forceFieldActive.SetBool(true));

            _timer.SetTimer(_bonusLenght);
            _timer.TimeIsOver += DeactivateForceField;
            _timer.StartTimer();

            _hud.ActivateBonusTimer();
        }

        private void DeactivateForceField()
        {
            Channel<ForceFieldEvent>.Raise(_forceFieldActive.SetBool(false));
            _timer.ResetTimer();
        }
    }

}



