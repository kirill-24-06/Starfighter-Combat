using Utils.Events.Channel.Static;

namespace Refactoring
{
    public struct ChangeScoreEvent: IEvent
    {
        public int Score {  get; private set; }

        public ChangeScoreEvent SetInt(int score)
        {
            Score = score;
            return this;
        }
    }
}

