using Zenject;
using UnityEngine;


namespace Refactoring
{
    [CreateAssetMenu(fileName = " Ace Installer", menuName = "ScriptableObjects/Test/Ace")]
    public partial class EnemyAceInstaller : ScriptableObjectInstaller<EnemyAceInstaller>
    {
        [SerializeField] private AceSettings _aceSettings;

        public override void InstallBindings()
        {
            Container
                .Bind<BossAbilitiesBuilder>()
                .AsTransient()
                .WhenInjectedInto<InterceptorWithAbilitiesStageBuilder>()
                .NonLazy();

            foreach (var stage in _aceSettings.Stages)
            {
                var concreteBuilder = stage.GetStageBuilder();


                Container
                    .Bind<IBossStageBuilder>()
                    .To(concreteBuilder)
                    .AsCached()
                    .WithArguments(stage)
                    .NonLazy();
            }
        }
    }
}
