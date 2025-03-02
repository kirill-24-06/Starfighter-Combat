using Utils.Events.Channel.Static;

namespace Refactoring
{
    public struct PauseEvent : IEvent
    {
        public bool Value { get; private set; }

        public PauseEvent(bool value) { Value = value; }
    }
}
