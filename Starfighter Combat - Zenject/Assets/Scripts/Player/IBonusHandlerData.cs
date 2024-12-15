using UnityEngine;

public interface IBonusHandlerData
{
    public GameObject NukePrefab { get; set; }

    public int NukesStartAmount { get; set; }

    public float BonusLenght { get; set; }

    public int TempInvunrabilityTime { get; }

    public int NukeCooldown { get; }
}