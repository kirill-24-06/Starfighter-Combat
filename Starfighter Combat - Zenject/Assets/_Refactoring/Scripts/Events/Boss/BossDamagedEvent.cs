using Utils.Events.Channel.Static;

namespace Refactoring
{
    public struct BossDamagedEvent : IEvent
    {
        public float BossHealth { get; private set; }

        public BossDamagedEvent(float bossHealth) => BossHealth = bossHealth;
    }
}