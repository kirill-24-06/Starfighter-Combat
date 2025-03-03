using UnityEngine;

namespace Refactoring
{
    public class PointMove : IMoveComponent,IDirectionReciever
    {
        private IMovableData _data;
        private Vector3 _direction;
        private readonly Transform _client;

        public PointMove(Transform client, IMovableData data)
        {
            _client = client;
            _data = data;
        }

        public void Move()
        {
            _client.position = Vector3.MoveTowards(_client.position, _direction, _data.Speed * Time.deltaTime);
        }

        public void RecieveDirection(Vector3 newDirection) => _direction = newDirection;
    }

    public interface IDirectionReciever
    {
        public void RecieveDirection(Vector3 newDirection);
    }

}
