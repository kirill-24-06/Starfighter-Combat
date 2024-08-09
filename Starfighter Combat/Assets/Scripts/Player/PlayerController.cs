using UnityEngine;

public class PlayerController
{

    public Vector3 InputDirection() // <= ToDo IInputSystem
    {
        Vector3 direction = Vector3.zero;

        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");

        return direction;
    }

    public bool ShootInput()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            return true;
        }
       
        return false;
    }

    public bool BonusInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            return true;
        }

        return false;
    }
}
