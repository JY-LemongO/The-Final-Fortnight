using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    private static Dictionary<Define.SpriteType, int> SortingOrdersDict = new()
    {
        {Define.SpriteType.Background, -1000},
        {Define.SpriteType.Foreground, 1000},
        {Define.SpriteType.Entity, 0},
        {Define.SpriteType.Weapon, 900},
        {Define.SpriteType.WorldUI, 1001}
    };

    private static Dictionary<float, WaitForSeconds> WaitForSecDict = new();

    public static int GetSortingOreder(Define.SpriteType type)
        => SortingOrdersDict.ContainsKey(type) ? SortingOrdersDict[type] : 0;

    public static WaitForSeconds GetCachedWaitForSeconds(float time) 
        => WaitForSecDict.ContainsKey(time) ? WaitForSecDict[time] : WaitForSecDict[time] = new WaitForSeconds(time);    
}