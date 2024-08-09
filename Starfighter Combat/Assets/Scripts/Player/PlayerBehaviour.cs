using UnityEngine;

public class PlayerBehaviour : ObjectBehaviour
{
    private IAttacker _playerAttackHandler;
    private IDamageble _playerDamageHandler;
    private PlayerController _playerController;

    private bool _isEquiped = false;


    public void Initialise()
    {
        _objectMoveHandler = new PlayerMover(this);
        _playerAttackHandler = new Attacker(this);
        _playerDamageHandler = new Damageble(this);
        _playerController = new PlayerController();
    }
   
    private void Update()
    {
        _objectMoveHandler.Move(_playerController.InputDirection(), ObjectInfo.Speed);
        CheckBorders();

        if (_playerController.ShootInput())
        {
            _playerAttackHandler.Fire(ObjectInfo.Projectile);
        }

        if (_playerController.BonusInput() && _isEquiped)
        {
            UseBonus();
        }
    }

    private void UseBonus()
    {

    }

    private void CheckBorders()
    {
        if (transform.position.x < -ObjectInfo.GameZoneBorders.x)
        {
            transform.position = new Vector3(-ObjectInfo.GameZoneBorders.x, transform.position.y, transform.position.z);
        }

        if (transform.position.x > ObjectInfo.GameZoneBorders.x)
        {
            transform.position = new Vector3(ObjectInfo.GameZoneBorders.x, transform.position.y, transform.position.z);
        }

        if (transform.position.y < -ObjectInfo.GameZoneBorders.y)
        {
            transform.position = new Vector3(transform.position.x, -ObjectInfo.GameZoneBorders.y, transform.position.z);
        }

        if (transform.position.y > ObjectInfo.GameZoneBorders.y)
        {
            transform.position = new Vector3(transform.position.x, ObjectInfo.GameZoneBorders.y, transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyWeapon"))
        {
            _playerDamageHandler.TakeDamage(1);
            ObjectPoolManager.ReturnObjectToPool(collision.gameObject);
        }
    }
}