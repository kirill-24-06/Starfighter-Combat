using Utils.Events.Channel.Static;

namespace Refactoring
{
    public struct BombsAmountUpdateEvent : IEvent
    {
        public int BombsAmount { get; private set; }

        public BombsAmountUpdateEvent SetInt(int bombsAmount)
        {
            BombsAmount = bombsAmount;
            return this;
        }
    }
}

