using System.Collections.Generic;
using UnityEngine;

namespace Refactoring
{
    public interface IBossStageBuilder
    {
        IBossStage Build(MonoBehaviour client, IReadOnlyProperty<int> currentHealth, List<IResetable> resetables);
    }
}