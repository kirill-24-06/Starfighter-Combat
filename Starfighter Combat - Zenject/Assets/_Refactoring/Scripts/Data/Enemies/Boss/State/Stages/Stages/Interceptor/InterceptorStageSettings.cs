using System;
using UnityEngine;
using Utils.Serializer;

namespace Refactoring
{
    [CreateAssetMenu(fileName = "Interceptor Stage Settings", menuName = "ScriptableObjects/BossSettings/BossStages/InterceptorStage")]
    public class InterceptorStageSettings : BossStageSettings, IAdvancedWeaponData, IAdvancedMovableData
    {
        [field: Header("Boss Settings")]
        [field: SerializeField] public InterfaceReference<IDamagebleData> HealthData { get; private set; }

        [field: Header("Weapon Settings")]
        [field: SerializeField] public float ReloadCountDown { get; private set; }
        [field: SerializeField] public AudioClip FireSound { get; private set; }
        [field: SerializeField, Range(0.1f, 1.0f)] public float FireSoundVolume { get; private set; }
        [field: SerializeField] public GameObject Projectile { get; private set; }
        [field: SerializeField] public int AttacksBeforeReposition { get; private set; }

        [field: Header("Movable Data")]
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public Vector3 Direction { get; private set; }
        [field: SerializeField] public Vector3 GameArea { get; private set; }
        [field: SerializeField] public float LowerYConstraint { get; private set; }

        [field: Header("Stage Switch Condition")]
        [field: SerializeField, Range(0.1f, 1.0f)] public float StageSwitchHealthPercent { get; private set; }

        public override Type GetStageBuilder()
        {
            return typeof(InterceptorStageBuilder);
        }
    }
}