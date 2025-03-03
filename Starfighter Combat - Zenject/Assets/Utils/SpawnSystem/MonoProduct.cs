using System;
using UnityEngine;


namespace Utils.SpawnSystem
{
    public abstract class MonoProduct : MonoBehaviour, IProduct<MonoProduct>
    {
        private Action<MonoProduct> _release;

        public bool IsConstructed { get; protected set; }

        public virtual MonoProduct OnGet() { return this; }
        public virtual MonoProduct OnRelease() { return this; }

        public MonoProduct WithRelease(Action<MonoProduct> returnAction)
        {
            if (_release != null) return this;

            _release = returnAction;
            return this;
        }
        protected void Release() => _release?.Invoke(this);
    }
}
