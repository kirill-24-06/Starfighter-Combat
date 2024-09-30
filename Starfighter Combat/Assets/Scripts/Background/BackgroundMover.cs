using UnityEngine;


public class BackgroundMover : MonoBehaviour
{
    [SerializeField,Range(0,1)] private float _paralaxCoeff = 1.0f;

    private float _offset;
    private Vector3 _startPosition;

    public void Initialise(float offset)
    {
        _startPosition = transform.position;
        _offset = offset;
    }

    public void Move(float speed)
    {
        transform.Translate(speed * _paralaxCoeff * Time.deltaTime * Vector3.down);

        if (transform.position.y < _startPosition.y - _offset)
        {
            transform.position = _startPosition;
        }
    }
}
