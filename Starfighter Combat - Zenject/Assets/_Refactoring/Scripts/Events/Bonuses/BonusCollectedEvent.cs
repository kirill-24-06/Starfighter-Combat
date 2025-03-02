using Utils.Events.Channel.Static;

namespace Refactoring
{
    public struct BonusCollectedEvent : IEvent
    {
        public BonusTag BonusTag { get; private set; }

        public BonusCollectedEvent SetTag(BonusTag tag)
        {
            BonusTag = tag;
            return this;
        }
    }
}
