using UnityEngine;

using Random = UnityEngine.Random;

namespace Refactoring
{
    public class RandomPointReposition : RepositionStrategy
    {
        private IAdvancedMovableData _data;

        public RandomPointReposition(IAdvancedMovableData data, IDirectionReciever directionSetter) : base(directionSetter)
        {
            _data = data;
        }

        public override Vector3 GetRepositionDirection() => SetNewDirection();

        private Vector3 SetNewDirection()
        {
            var x = Random.Range(-_data.GameArea.x, _data.GameArea.x);
            var y = Random.Range(_data.LowerYConstraint, _data.GameArea.y);

            _moveDirection.x = x;
            _moveDirection.y = y;

            _directionSetter.RecieveDirection(_moveDirection);

            return _moveDirection;
        }
    }
}