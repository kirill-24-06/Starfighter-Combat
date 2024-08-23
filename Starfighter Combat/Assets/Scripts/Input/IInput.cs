using UnityEngine;

public interface IInput
{
    public Vector3 MoveInput();

    public bool ShootInput();

    public bool BonusInput();

    public bool PauseInput();
}