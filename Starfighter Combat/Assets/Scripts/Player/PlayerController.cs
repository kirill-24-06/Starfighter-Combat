using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Player _player;

    private EventManager _events;
    private IInput _inputHandler;
    private IAttacker _attackHandler;


    private bool _isGameActive;

    public void Initialise()
    {
        _events = EventManager.GetInstance();

        _inputHandler = new KeyboardInput();
        _attackHandler = new PlayerAttacker(_player);

        _events.Start += OnStart;
        _events.Stop += OnStop;
        _events.Multilaser += EnableMultilaser;
    }

    private void OnStart()
    {
        _isGameActive = true;
    }

    private void OnStop()
    {
        _isGameActive = false;
    }

    private void Update()
    {
        if (!_isGameActive)
            return;

        Move(_inputHandler.MoveInput(),_player.PlayerData.Speed);
        CheckBorders();

        if (_inputHandler.ShootInput())
        {
            _attackHandler.Fire(_player.PlayerData.Projectile);
        }

        if (_inputHandler.BonusInput() && _player.IsEquiped)
        {
            UseSpehre();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ObjectHolder.GetInstance().FindRegisteredObject(collision.gameObject, ObjectTag.EnemyWeapon) ||
            ObjectHolder.GetInstance().FindRegisteredObject(collision.gameObject, ObjectTag.Enemy))
        {
            if (!_player.IsInvunerable && !_player.IsDroneActive)
            {
                _events.PlayerDamaged?.Invoke(1);
            }

            else if (!_player.IsInvunerable && _player.IsDroneActive)
            {
                _events.DroneDestroyed?.Invoke();
            }

            ObjectPoolManager.ReturnObjectToPool(collision.gameObject);
        }
    }

    private void OnDisable()
    {
        _attackHandler = new PlayerAttacker(_player);
    }

    public void Move(Vector3 direction, float speed)
    {
        direction = direction.normalized;
        _player.transform.Translate(speed * Time.deltaTime * direction);
    }

    private void EnableMultilaser(bool isEnabled)
    {
        if(isEnabled)
        {
            _attackHandler = new PlayerAttackerMultiple(_player);
        }

        else if (!isEnabled)
        {
            _attackHandler = new PlayerAttacker(_player);
        }
    }

    private void UseSpehre()
    {
        _events.IonSphereUse?.Invoke();
    }

    private void CheckBorders()
    {
        if (transform.position.x < -_player.PlayerData.GameZoneBorders.x)
        {
            transform.position = new Vector3(-_player.PlayerData.GameZoneBorders.x, transform.position.y, transform.position.z);
        }

        if (transform.position.x > _player.PlayerData.GameZoneBorders.x)
        {
            transform.position = new Vector3(_player.PlayerData.GameZoneBorders.x, transform.position.y, transform.position.z);
        }

        if (transform.position.y < -_player.PlayerData.GameZoneBorders.y)
        {
            transform.position = new Vector3(transform.position.x, -_player.PlayerData.GameZoneBorders.y, transform.position.z);
        }

        if (transform.position.y > _player.PlayerData.GameZoneBorders.y)
        {
            transform.position = new Vector3(transform.position.x, _player.PlayerData.GameZoneBorders.y, transform.position.z);
        }
    }

    private void OnDestroy()
    {
        _events.Start -= OnStart;
        _events.Stop -= OnStop;
    }
}