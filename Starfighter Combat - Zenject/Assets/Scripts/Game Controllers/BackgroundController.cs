using UnityEngine;
using System.Collections;
using Zenject;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private BackgroundMover[] _layers;
    [SerializeField] private float _speed;

    private WaitForSeconds _delay;
    private bool _isGameActive = false;

    private EventManager _events;

    [Inject]
    public void Construct(EventManager events)
    {
        _events = events;
        _events.Stop += OnStop;

        float offset = _layers[0].GetComponentInChildren<SpriteRenderer>().bounds.size.y;

        foreach (var layer in _layers)
            layer.Initialise(offset, _speed);

        _delay = new WaitForSeconds(2.0f);
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

    private void OnStop() => StartCoroutine(Stop());

    private IEnumerator Stop()
    {
        yield return _delay;
        _isGameActive = false;
    }

    private void OnDestroy() => _events.Stop -= OnStop;
}