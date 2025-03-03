namespace Refactoring
{
    public abstract class BossStage : IBossStage
    {
        public virtual bool StageCompleted { get; protected set; } = false;

        public virtual void Enter() { }

        public virtual void Exit() { }

        public virtual void FixedUpdate() { }

        public virtual void Update() { }

    }
}