using System;
using UnityEngine;

namespace Refactoring
{
    [CreateAssetMenu(fileName = "Interceptor With Abilities Stage Settings", menuName = "ScriptableObjects/BossSettings/BossStages/InterceptorWithAbilitiesStage")]
    public class InterceptorWithAbilitiesStageSettings : InterceptorStageSettings, IAbilityCasterData
    {
        public enum Ability
        {
            Shield,
            MissileLaunch,
            SpawnMinion
        }

        [field: Header("Ability Caster Settings")]
        [field: SerializeField] public Ability[] BossAbilities { get; private set; }
        [field: SerializeField] public float Cooldown { get; private set; }
        [field: SerializeField] public BossAbilitiesSettings AbilitiesSettings { get; private set; }

        public override Type GetStageBuilder()
        {
            return typeof(InterceptorWithAbilitiesStageBuilder);
        }
    }
}