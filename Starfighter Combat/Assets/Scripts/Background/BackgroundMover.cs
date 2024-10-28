using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float _paralaxCoeff = 1.0f;

    private float _offset;
    private Vector3 _startPosition;
    private Transform _transform;

    public float ParalaxCoeff => _paralaxCoeff;

    public void Initialise(float offset)
    {
        _transform = transform;
        _startPosition = _transform.position;
        _offset = offset;
    }

    public void Move(Vector3 transition)
    {
        _transform.position += transition;

        if (_transform.position.y < _startPosition.y - _offset)
            _transform.position = _startPosition;
    }
}
