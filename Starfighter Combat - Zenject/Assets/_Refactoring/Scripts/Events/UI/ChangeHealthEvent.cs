using Utils.Events.Channel.Static;

namespace Refactoring
{
    public struct ChangeHealthEvent : IEvent
    {
        public int Health { get; private set; }

        public ChangeHealthEvent(int health)
        {
            Health = health;
        }
    }
}

