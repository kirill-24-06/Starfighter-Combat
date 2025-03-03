using Utils.StateMachine;

namespace Refactoring
{
    public class BossCombatState : IState
    {
        private IBossStage[] _bossStages;

        private IBossStage _currentStage;
        private int _currentStageIndex;

        private int _amountOfStages;

        public BossCombatState(IBossStage[] bossStages)
        {
            _bossStages = bossStages;

            _currentStageIndex = 0;

            _currentStage = _bossStages[_currentStageIndex];

            _amountOfStages = _bossStages.Length;
        }

        public void Enter() => _currentStage.Enter();

        public void FixedUpdate() => _currentStage.FixedUpdate();

        public void Update()
        {
            CheckCompletion();
            _currentStage.Update();
        }

        public void Exit()
        {
            _currentStage.Exit();
            _currentStageIndex = 0;
        }

        private void CheckCompletion()
        {
            if (_currentStageIndex + 1 >= _amountOfStages) return;

            if(_currentStage.StageCompleted)
            {
                SwitchStage();
            }
        }

        private void SwitchStage()
        {
            _currentStage.Exit();

            _currentStageIndex++;

            _currentStage = _bossStages[_currentStageIndex];

            _currentStage.Enter();
        }
    }

}