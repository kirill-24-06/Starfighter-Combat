using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Utils.SpawnSystem;

namespace Refactoring
{
    public class AnimationEffect : MonoProduct
    {
        private AnimationEffectSettings _settings;

        private CancellationToken _token;

        public void Construct(AnimationEffectSettings settings, CancellationToken token)
        {
            _settings = settings;
            _token = token;


            IsConstructed = true;
        }

        public AnimationEffect Deactivate()
        {
            UniTask
                .Delay(TimeSpan.FromSeconds(_settings.DestroyTime), cancellationToken: _token)
                .ContinueWith(Release)
                .Forget();

            return this;
        }
    }
}
