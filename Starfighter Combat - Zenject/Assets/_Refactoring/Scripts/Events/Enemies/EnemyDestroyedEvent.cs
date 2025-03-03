using Utils.Events.Channel.Static;

namespace Refactoring
{
    public struct EnemyDestroyedEvent:IEvent
    {
        public EnemyDestroyedEvent(EnemyStrenght enemyStrenght)
        {
            EnemyStrenght = enemyStrenght;
        }

        public EnemyStrenght EnemyStrenght {  get; set; }
    }


}

