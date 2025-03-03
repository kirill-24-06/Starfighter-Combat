using Utils.Events.Channel.Static;

namespace Refactoring
{
    public struct PlayerDamagedEvent : IEvent
    {
        public int Damage;

        public PlayerDamagedEvent(int damage) { Damage = damage;}
    }
   
}

