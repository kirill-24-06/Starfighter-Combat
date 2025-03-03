using System;
using UnityEngine;

namespace Refactoring
{
    public class BackgroundMover : MonoBehaviour
    {
        [SerializeField, Range(0, 1)] private float _paralaxCoeff = 1.0f;

        private float _speed;
        private float _offset;
        private Vector3 _startPosition;
        private Transform _transform;

        public void Initialise(float offset, float speed)
        {
            _speed = speed;
            _transform = transform;
            _startPosition = _transform.position;
            _offset = offset;
        }

        public void Move()
        {
            var speed = _speed * _paralaxCoeff * Time.deltaTime;
            _transform.Translate(speed * Vector3.down);

            if (_transform.position.y < _startPosition.y - _offset)
                _transform.position = _startPosition;
        }
    }
}

