using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _laser;
    [SerializeField] private Transform _laserSpawnPosition;

    [SerializeField] private float _playerSpeed;
    private PlayerMover _playerMover;

    private float _xBorder = 19.0f;
    private float _yBorder = 10.0f;

    private bool _isShooted = false;
    private bool _isEquiped = false;

    public void Initialise()
    {
        _playerMover = new PlayerMover(this);
    }

    private void Update()
    {
        _playerMover.Move(InputDirection(), _playerSpeed);
        CheckBorders();

        if (Input.GetKeyDown(KeyCode.Space) && !_isShooted)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.F) && _isEquiped)
        {
            UseBonus();
        }
    }

    private Vector3 InputDirection() // <= ToDo IInputSystem
    {
        Vector3 direction = Vector3.zero;

        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");

        return direction;
    }

    private void Shoot()
    {
        ObjectPoolManager.SpawnObject(_laser, _laserSpawnPosition.transform.position, _laser.transform.rotation);

        _isShooted = !_isShooted;
        StartCoroutine(Reload());
    }

    private void UseBonus()
    {
        //To Do
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(0.2f);
        _isShooted = !_isShooted;
    }

    private void CheckBorders()
    {
        if (transform.position.x < -_xBorder)
        {
            transform.position = new Vector3(-_xBorder, transform.position.y, transform.position.z);
        }

        if (transform.position.x > _xBorder)
        {
            transform.position = new Vector3(_xBorder, transform.position.y, transform.position.z);
        }

        if (transform.position.y < -_yBorder)
        {
            transform.position = new Vector3(transform.position.x, -_yBorder, transform.position.z);
        }

        if (transform.position.y > _yBorder)
        {
            transform.position = new Vector3(transform.position.x, _yBorder, transform.position.z);
        }
    }
}
