using Utils.Events.Channel.Static;

namespace Refactoring
{
    public class RepairBonus : IBonus
    {
        private PlayerHealedEvent _playerHealed;

        public RepairBonus(int healthAmount)
        {
           _playerHealed = new PlayerHealedEvent().SetInt(healthAmount);
        }

        public void Handle() => Channel<PlayerHealedEvent>.Raise(_playerHealed);
    }

}



