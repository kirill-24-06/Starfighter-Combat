using System.Threading;
using Utils.Pool.Generic;
using Utils.SpawnSystem;

namespace Refactoring
{
    public class VisualEffectsFactory : GenericFactory<AnimationEffect, AnimationEffectSettings>
    {
        private CancellationToken _token;

        public VisualEffectsFactory(AnimationEffectSettings settings ,CancellationToken token)
        {
            _settings = settings;
            _token = token;

            _pool = new CustomPool<AnimationEffect>(
                _settings.Create,
                _settings.OnGet,
                _settings.OnRelease,
                _settings.PrewarmAmount);
        }

        public override MonoProduct Create()
        {
            var effect = _pool.Get();

            return !effect.IsConstructed ? Build(effect).WithRelease(Release): effect.Deactivate();
        }

        protected override MonoProduct Build(AnimationEffect product)
        {
            product.Construct(_settings, _token);

            return product.Deactivate();
        }
    }
}
