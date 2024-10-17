using System.Collections.Generic;
using UnityEngine;

public class GlobalConstants
{
    //Conversion
    public static readonly float FloatConverter = 1.0f;
    public static readonly int MillisecondsConverter = 1000;

    // SpawnZones Data
    public static readonly Dictionary<string, AreaTag> AreaTagsByName = new()
    {
        {"SpawnAreaUpper",AreaTag.Upper},
        {"SpawnAreaLeft",AreaTag.Left},
        {"SpawnAreaRight",AreaTag.Right}
    };

    public static readonly Dictionary<AreaTag, Quaternion> AreaRotationsByTag = new()
    {
        {AreaTag.Upper,Quaternion.Euler(0, 0, 180)},
        {AreaTag.Left, Quaternion.Euler(0, 0,-90)},
        {AreaTag.Right, Quaternion.Euler(0, 0,90) }
    };

    //Collision damage
    public static readonly int CollisionDamage = 1;


    //Scenes

    public static readonly string MainSceneName = "MainScene";

    public static readonly string MainMenuSceneName = "MainMenu";
}
