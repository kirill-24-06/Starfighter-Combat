using Utils.Events.Channel.Static;

namespace Refactoring
{
    public struct BombsAddedEvent : IEvent
    {
        public int Amount {  get; private set; }

        public BombsAddedEvent SetInt(int amount)
        {
            Amount = amount;
            return this;
        }
    }
   
}



