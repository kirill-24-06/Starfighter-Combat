using Utils.Pool.Generic;


namespace Utils.SpawnSystem
{
    public abstract class GenericFactory<TProduct, TSettings> : IFactory<MonoProduct> where TSettings : ISettings<TProduct> where TProduct : MonoProduct
    {
        protected TSettings _settings;
        protected CustomPool<TProduct> _pool;

        public abstract MonoProduct Create();

        protected abstract MonoProduct Build(TProduct product);

        protected void Release(MonoProduct product) => _pool.Release((TProduct)product);

    }
}
