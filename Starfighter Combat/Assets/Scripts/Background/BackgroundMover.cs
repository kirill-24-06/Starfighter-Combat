using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private float _offset;
    private Vector3 _startPosition;

    public void Initialise()
    {
        _startPosition = transform.position;
        _offset = GetComponent<SpriteRenderer>().bounds.size.y / 2;
    }
    
    private void Update()
    {
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
}
