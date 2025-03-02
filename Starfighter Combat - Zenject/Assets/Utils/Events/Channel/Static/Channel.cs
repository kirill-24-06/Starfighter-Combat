namespace Utils.Events.Channel.Static
{
    public static class Channel<T> where T : IEvent
    {
        public delegate void Event(T @event);

        public static event Event OnEvent;

        public static void Raise(T @event) => OnEvent?.Invoke(@event);
    }

    public interface IEvent { }
}
