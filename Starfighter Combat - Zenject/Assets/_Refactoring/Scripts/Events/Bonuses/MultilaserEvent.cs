using Utils.Events.Channel.Static;

namespace Refactoring
{
    public struct MultilaserEvent:IEvent
    {
        public bool Enabled {  get; private set; }

        public MultilaserEvent SetBool(bool enabled)
        {
            Enabled = enabled;
            return this;
        }
    }
}
