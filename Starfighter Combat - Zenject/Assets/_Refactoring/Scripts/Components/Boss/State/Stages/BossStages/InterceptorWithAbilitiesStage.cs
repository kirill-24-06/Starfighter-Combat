using System.Collections;
using UnityEngine;
using Utils.Extensions;
using Utils.StateMachine;

namespace Refactoring
{
    public class InterceptorWithAbilitiesStage : InterceptorStage
    {
        private MonoBehaviour _context;
        private GameObject _contextGO;

        private IAbilityCasterData _abilityCasterData;
        private BossAbilitiesComponent _bossAbilities;
        private IEnumerator _useAbility;

        public InterceptorWithAbilitiesStage(
            MonoBehaviour client,
            IAbilityCasterData abilityCasterData,
            BossAbilitiesComponent bossAbilities,
            IPredicate stageCompleteCondition,
            StateMachine stateMachine,
            IState baseState) : base(stageCompleteCondition, stateMachine, baseState)
        {
            _context = client;
            _contextGO = _context.gameObject;

            _abilityCasterData = abilityCasterData;
            _bossAbilities = bossAbilities;

            _useAbility = UseAbility();
        }

        public override void Enter()
        {
            base.Enter();
            _context.StartCoroutine(_useAbility);
        }

        public override void Exit()
        {
            base.Exit();
            _context.StopCoroutine(_useAbility);
        }

        private IEnumerator UseAbility()
        {
            while (_contextGO.activeInHierarchy)
            {
                _bossAbilities.UseAbility();
                yield return WaitFor.Seconds(_abilityCasterData.Cooldown);
            }
        }
    }
}