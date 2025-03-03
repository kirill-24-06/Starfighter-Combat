using UnityEngine;

using Random = UnityEngine.Random;

namespace Refactoring
{
    public class TwoPointsReposition : RepositionStrategy, IResetable
    {
        Transform _client;

        private Bounds[] _patrolAreas;
        private Vector3[] _points;
        private int _pointIndex;

        private bool _inCombat = false;

        public TwoPointsReposition(Transform client, Bounds[] patrolAreas, IDirectionReciever directionSetter) : base(directionSetter)
        {
            _client = client;
            _patrolAreas = patrolAreas;
            _points = new Vector3[_patrolAreas.Length];
        }

        public override Vector3 GetRepositionDirection() => SetNewDirection();

        private Vector3 SetNewDirection()
        {
            if (!_inCombat)
            {
                GetNewMovePoints();
                _moveDirection = _points[_pointIndex];

                _inCombat = true;
            }

            else
            {
                UpdatePoints();
            }

            _directionSetter.RecieveDirection(_moveDirection);

            return _moveDirection;
        }

        private void UpdatePoints()
        {
            _pointIndex = (_pointIndex == 1) ? 0 : 1;
            _moveDirection = _points[_pointIndex];
        }

        private void GetNewMovePoints()
        {
            for (int i = 0; i < _points.Length; i++)
            {
                _points[i] = GenerateMovePoint(_patrolAreas[i]);
            }

            SetFirstPoint();
        }

        private void SetFirstPoint()
        {
            var maxDistance = float.MaxValue;
            int index = 0;

            for (int i = 0; i < _points.Length; i++)
            {
                var distance = Vector3.Distance(_client.position, _points[i]);

                if (distance < maxDistance)
                {
                    maxDistance = distance;
                    index = i;
                }
            }

            _pointIndex = index;
        }

        private Vector3 GenerateMovePoint(Bounds area)
        {
            var x = Random.Range(area.min.x, area.max.x);
            var y = Random.Range(area.min.y, area.max.y);

            return new Vector3(x, y);
        }

        public void Reset() => _inCombat = false;
    }
}