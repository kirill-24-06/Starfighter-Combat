using UnityEngine;
using System.Collections;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private BackgroundMover[] _layers;
    [SerializeField] private float _speed;

    private bool _isGameActive = false;

    public void Initialise()
    {
        // all tiles must be same size
        float offset = _layers[0].GetComponentInChildren<SpriteRenderer>().bounds.size.y;

        foreach (var layer in _layers)
            layer.Initialise(offset);

        EntryPoint.Instance.Events.Start += OnStart;
        EntryPoint.Instance.Events.Stop += OnStop;
    }

    private void Update()
    {
        if (!_isGameActive) return;

        Move();
    }

    private void Move()
    {
        foreach (var layer in _layers)
            layer.Move(_speed);
    }

    private void Start()
    {
        _isGameActive = true;
    }
    private void OnStart() => _isGameActive = true;

    private void OnStop() => StartCoroutine(Stop());

    private IEnumerator Stop()
    {
        yield return new WaitForSeconds(2.0f);
        _isGameActive = false;
    }

    private void OnDestroy()
    {
        EntryPoint.Instance.Events.Start -= OnStart;
        EntryPoint.Instance.Events.Stop -= OnStop;
    }
}
