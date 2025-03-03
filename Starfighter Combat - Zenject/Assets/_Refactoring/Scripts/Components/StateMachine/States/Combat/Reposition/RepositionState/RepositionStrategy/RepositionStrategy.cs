using UnityEngine;

namespace Refactoring
{
    public abstract class RepositionStrategy : IRepositionStrategy
    {
        protected IDirectionReciever _directionSetter;
        protected Vector3 _moveDirection;

        public RepositionStrategy(IDirectionReciever directionSetter)
        {
            _directionSetter = directionSetter;
        }

        public abstract Vector3 GetRepositionDirection();
    }
}