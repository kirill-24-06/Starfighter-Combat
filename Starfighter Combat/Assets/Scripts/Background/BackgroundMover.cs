using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float _paralaxCoeff = 1.0f;

    private float _offset;
    private Vector3 _startPosition;
    private Transform _transform;

    public void Initialise(float offset)
    {
        _transform = transform;
        _startPosition = _transform.position;
        _offset = offset;
    }

    public void Move(float speed)
    {
        _transform.Translate(speed * _paralaxCoeff * Time.deltaTime * Vector3.down);

        if (_transform.position.y < _startPosition.y - _offset)
            _transform.position = _startPosition;
    }
}
