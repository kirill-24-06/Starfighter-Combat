using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Refactoring
{
    public class AwakeProvider : MonoBehaviour
    {
        private List<IAwakeable> _awakeables;

        [Inject]
        public void Construct(List<IAwakeable> awakeables)
        {
            _awakeables = awakeables;
        }


        private void Awake()
        {
            if (_awakeables != null)
            {
                OnAwake();
            }

            Destroy(this);
        }

        private void OnAwake()
        {
            foreach (var awakeable in _awakeables)
            {
                awakeable.Awake();
            }
        }
    }

    public interface IAwakeable
    {
        void Awake();
    }

}
