using Utils.Events.Channel.Static;

namespace Refactoring
{
    public class NukeBonus : IBonus
    {
        private BombsAddedEvent _addBomb;

        public NukeBonus(int nukesAmount)
        {
            _addBomb = new BombsAddedEvent().SetInt(nukesAmount);
        }
        public void Handle() => Channel<BombsAddedEvent>.Raise(_addBomb);
    }

}



