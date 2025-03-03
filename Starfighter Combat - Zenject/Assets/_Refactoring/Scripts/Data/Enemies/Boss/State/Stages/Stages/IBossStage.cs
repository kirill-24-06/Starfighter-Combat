using Utils.StateMachine;

namespace Refactoring
{
    public interface IBossStage : IState
    {
        bool StageCompleted { get; }
    }
}