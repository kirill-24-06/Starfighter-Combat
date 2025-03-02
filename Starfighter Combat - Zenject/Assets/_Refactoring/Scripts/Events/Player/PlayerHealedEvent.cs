using Utils.Events.Channel.Static;

namespace Refactoring
{
    public struct PlayerHealedEvent : IEvent
    {
        public int Health {  get; private set; }

        public PlayerHealedEvent SetInt(int value)
        {
            Health = value;
            return this;
        }
    }
}
