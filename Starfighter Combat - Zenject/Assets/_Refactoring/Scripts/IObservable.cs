using System;

namespace Refactoring
{
    public interface IObservable<T>
    {
        public event Action<T> OnValueChanged;
    }

    public interface IReadOnlyProperty<T> : IObservable<T>
    {
        public T Value { get; }
    }
}