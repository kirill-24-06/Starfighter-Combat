using Utils.Events.Channel.Static;

namespace Refactoring
{
    public struct AddScoreEvent : IEvent
    {
        public int Score { get; set; }
        public AddScoreEvent(int score) => Score = score;
    }
}

