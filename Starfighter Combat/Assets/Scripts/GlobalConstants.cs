using System.Collections.Generic;
using UnityEngine;

public class GlobalConstants
{

    // Данные зон спавна
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

    public static readonly int CollisionDamage = 1; 
}
