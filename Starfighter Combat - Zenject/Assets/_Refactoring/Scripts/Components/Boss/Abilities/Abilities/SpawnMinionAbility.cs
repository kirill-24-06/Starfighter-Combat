using Utils.Events.Channel.Static;
using Random = UnityEngine.Random;

namespace Refactoring
{
    public class SpawnMinionAbility : IBossAbility
    {
        private MinionSettings[] _minions;

        public SpawnMinionAbility(MinionSettings[] minions)
        {
            _minions = minions;
        }

        public void Cast()
        {
            var index = Random.Range(0, _minions.Length);

            for (int i = 0; i < _minions[index].AmountToSpawn; i++)
            {
                var minion = _minions[index].Minion.Value;

                Channel<SpawnMinionEvent>.Raise(new SpawnMinionEvent(minion));
            }
        }
    }
}