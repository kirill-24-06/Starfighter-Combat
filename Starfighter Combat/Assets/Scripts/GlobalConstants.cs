using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalConstants
{

    // ������ ��� ������
    public static readonly Dictionary<string, AreaTag> AreaTagsByName = new Dictionary<string, AreaTag>()
    {
        {"SpawnAreaUpper",AreaTag.Upper},
        {"SpawnAreaLeft",AreaTag.Left},
        {"SpawnAreaRight",AreaTag.Right}
    };

    public static readonly Dictionary<AreaTag, Quaternion> AreaRotationsByTag = new Dictionary<AreaTag, Quaternion>()
    {
        {AreaTag.Upper,Quaternion.Euler(0, 0, 180)},
        {AreaTag.Left, Quaternion.Euler(0, 0,-90)},
        {AreaTag.Right, Quaternion.Euler(0, 0,90) }
    };
}
