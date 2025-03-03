using Utils.Events.Channel.Static;

namespace Refactoring
{
    public struct ForceFieldEvent : IEvent
    {
        public bool IsActive { get; private set; }

        public ForceFieldEvent SetBool(bool value)
        {
            IsActive = value;
            return this;
        }
    }

}

