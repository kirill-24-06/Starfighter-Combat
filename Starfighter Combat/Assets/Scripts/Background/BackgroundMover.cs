using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private float _offset;
    private Vector3 _startPosition;

    private bool _isGameActive = false;

    public void Initialise()
    {
        _startPosition = transform.position;
        _offset = GetComponent<SpriteRenderer>().bounds.size.y / 2;

        EntryPoint.Instance.Events.Start += OnStart;
        EntryPoint.Instance.Events.Stop += OnStop;
    }

    private void Update()
    {
        if (!_isGameActive)
            return;

        Move();

        if (transform.position.y < _startPosition.y - _offset)
        {
            transform.position = _startPosition;
        }
    }

    private void Move()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down);
    }

    private void OnStart()
    {
        _isGameActive = true;
    }

    private void OnStop()
    {
        StartCoroutine(Stop());
    }

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
