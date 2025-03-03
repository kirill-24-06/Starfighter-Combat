using UnityEngine;

namespace Refactoring
{
    public class BasicMoveComponent : IMoveComponent
    {
        private readonly Transform _client;
        private readonly IMovableData _data;

        public BasicMoveComponent(Transform client,IMovableData data)
        {
            _client = client;
            _data = data;
        }

        public void Move()
        {
            _client.Translate(_data.Speed * Time.deltaTime * _data.Direction);
        }
    }


}

