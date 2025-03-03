using Utils.Events.Channel.Static;

namespace Refactoring
{
    public struct BossInvunrableEvent : IEvent
    {
        public bool Value { get; private set; }

        public BossInvunrableEvent(bool value) => Value = value;
    }
}