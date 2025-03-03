using System;
using UnityEngine;

namespace Refactoring
{
    public abstract class BossStageSettings : ScriptableObject
    {
        public abstract Type GetStageBuilder();
    }
}