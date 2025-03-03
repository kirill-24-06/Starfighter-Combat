using UnityEngine;

namespace Refactoring
{
    public class MissileMoveComponent : IMoveComponent, IDirectionReciever
    {
        private readonly Transform _client;
        private IMovableData _data;
        private Vector3 _direction;

        public MissileMoveComponent(Transform client, IMovableData data)
        {
            _client = client;
            _data = data;
        }

        public void Move()
        {
            _client.transform.position += _data.Speed * Time.deltaTime * _direction;
        }

        public void RecieveDirection(Vector3 newDirection)
        {
            _direction = (newDirection - _client.transform.position).normalized;
        }
    }
}
