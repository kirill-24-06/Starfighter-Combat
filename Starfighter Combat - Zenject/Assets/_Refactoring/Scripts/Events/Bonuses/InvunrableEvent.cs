using Utils.Events.Channel.Static;

namespace Refactoring
{
    public struct InvunrableEvent : IEvent
    {
        public bool IsInvunrable { get; private set; }

        public InvunrableEvent SetBool(bool value)
        {
            IsInvunrable = value;
            return this;
        }
    }

}

