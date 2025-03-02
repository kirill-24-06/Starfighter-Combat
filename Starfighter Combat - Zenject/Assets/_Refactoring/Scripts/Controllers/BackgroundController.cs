using UnityEngine;
using Utils.Events.Channel.Static;
using System.Collections;

namespace Refactoring
{
    public class BackgroundController : MonoBehaviour
    {
        [SerializeField] private BackgroundMover[] _layers;
        [SerializeField] private float _speed;
        [SerializeField] private float _stopDelay = 2.0f;

        private WaitForSeconds _delay;
        private bool _isGameActive = false;


        private void Awake()
        {
            Channel<StopEvent>.OnEvent += OnStop;

            float offset = _layers[0].GetComponentInChildren<SpriteRenderer>().bounds.size.y;

            foreach (var layer in _layers)
            {
                layer.Initialise(offset, _speed);
            }

            _delay = new WaitForSeconds(_stopDelay);
        }

        private void Update()
        {
            if (!_isGameActive) return;
            Move();
        }

        private void Move()
        {
            for (int i = 0; i < _layers.Length; i++)
                _layers[i].Move();
        }

        private void Start() => _isGameActive = true;

        private void OnStop(StopEvent @event) => StartCoroutine(Stop());

        private IEnumerator Stop()
        {
            yield return _delay;
            _isGameActive = false;
        }

        private void OnDestroy() => Channel<StopEvent>.OnEvent -= OnStop;
    }
}

