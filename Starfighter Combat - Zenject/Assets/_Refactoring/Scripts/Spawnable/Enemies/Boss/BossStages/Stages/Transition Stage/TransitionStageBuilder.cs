using System.Collections.Generic;
using UnityEngine;

namespace Refactoring
{
    public class TransitionStageBuilder : IBossStageBuilder
    {
        private TransitionStageSettings _settings;

        public TransitionStageBuilder(TransitionStageSettings settings)
        {
            _settings = settings;
        }

        public IBossStage Build(MonoBehaviour client, IReadOnlyProperty<int> currentHealth, List<IResetable> resetables)
        {
            var shield = client.transform.Find("TransitionStageShield").gameObject;

            var stage = new TransitionStage(_settings.Minions , shield);

            return stage;
        }
    }
}