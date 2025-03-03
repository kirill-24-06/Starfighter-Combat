using System.Collections.Generic;
using System;

namespace Utils.Pool.Generic
{
    public class CustomPool<T>
    {
        private readonly Stack<T> _pool;

        private Func<T> _create;
        private Action<T> _onGet;
        private Action<T> _onRelease;

        public CustomPool(Func<T> create, Action<T> onGet = null, Action<T> onRelease = null, int startCapacity = 3)
        {
            _create = create;
            if (onGet != null) _onGet = onGet;
            if (onRelease != null) _onRelease = onRelease;

            _pool = new Stack<T>();

            for (int i = 0; i < startCapacity; i++)
            {
                var obj = Create();
                _pool.Push(obj);
            }
        }

        public T Get()
        {
            var obj = _pool.Count > 0 ? _pool.Pop() : Create();
            _onGet?.Invoke(obj);
            return obj;
        }

        public void Release(T obj)
        {
            _onRelease?.Invoke(obj);
            _pool.Push(obj);
        }

        private T Create() => _create.Invoke();
    }


}
