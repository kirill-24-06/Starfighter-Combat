using UnityEngine;
using Utils.Events.Channel.Static;

namespace Refactoring
{
    public class ForceFieldBehaviour : MonoBehaviour
    {
        private InvunrableEvent _invunrableEvent;
        private GameObject _gameObject;

        public bool ShieldActive { get; private set; }

        public GameObject GameObject => _gameObject;

        private void Awake()
        {
            _gameObject  = gameObject;
        }

        private void Start()
        {
            Channel<ForceFieldEvent>.OnEvent += Handle;
            gameObject.SetActive(false);
        }

        private void Handle(ForceFieldEvent @event)
        {
            if (@event.IsActive)
                OnActivated();

            else
                OnDeactivated();
        }

        private void OnActivated()
        {
            _gameObject.SetActive(true);
            Channel<InvunrableEvent>.Raise(_invunrableEvent.SetBool(true));
            ShieldActive = true;
        }

        private void OnDeactivated()
        {
            _gameObject.SetActive(false);
            Channel<InvunrableEvent>.Raise(_invunrableEvent.SetBool(false));

            ShieldActive = false;
        }

        private void OnDestroy()
        {
            Channel<ForceFieldEvent>.OnEvent -= Handle;
        }
    }

}

