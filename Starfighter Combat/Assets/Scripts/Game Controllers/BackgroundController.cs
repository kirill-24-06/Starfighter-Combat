using UnityEngine;
using System.Collections;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private BackgroundMover[] _layers;
    [SerializeField] private float _speed;

    private WaitForSeconds _delay;
    private Vector3[] _transitions;
    private bool _isGameActive = false;

    public void Initialise()
    {
        // all tiles must be same size
        float offset = _layers[0].GetComponentInChildren<SpriteRenderer>().bounds.size.y;
        _transitions = new Vector3[_layers.Length];

        foreach (var layer in _layers)
            layer.Initialise(offset);

        EntryPoint.Instance.Events.Stop += OnStop;
        _delay = new WaitForSeconds(2.0f);
    }

    private void Update()
    {
        if (!_isGameActive) return;

        Calculate();
        Move();
    }

    private void Calculate()
    {
        for (int i = 0; i < _transitions.Length; i++)
        {
            _transitions[i] = _speed * _layers[i].ParalaxCoeff * Time.deltaTime * Vector3.down;
        }
    }

    private void Move()
    {
        for (int i = 0; i < _layers.Length; i++)
        {
            _layers[i].Move(_transitions[i]);
        }
    }

    private void Start() => _isGameActive = true;

    private void OnStop() => StartCoroutine(Stop());

    private IEnumerator Stop()
    {
        yield return _delay;
        _isGameActive = false;
    }

    private void OnDestroy() => EntryPoint.Instance.Events.Stop -= OnStop;
}