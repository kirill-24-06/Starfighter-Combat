using Utils.Events.Channel.Static;

namespace Refactoring
{
    public class DroneBonus : IBonus
    {
        private DroneAddedEvent _droneAdded;
        public void Handle() => Channel<DroneAddedEvent>.Raise(_droneAdded);
    }
}
