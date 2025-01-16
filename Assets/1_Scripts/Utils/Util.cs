using System.Collections.Generic;
using UnityEngine;

public enum SpriteType
{
    Background,    
    Foreground,
    Entity,
    Weapon,
    WorldUI,
}

public static class Util
{
    private static Dictionary<SpriteType, int> SortingOrdersDict = new()
    {
        {SpriteType.Background, -1000},
        {SpriteType.Foreground, 1000},
        {SpriteType.Entity, 0},
        {SpriteType.Weapon, 900},
        {SpriteType.WorldUI, 1001}
    };

    private static Dictionary<float, WaitForSeconds> WaitForSecDict = new();

    public static int GetSortingOreder(SpriteType type)
        => SortingOrdersDict.ContainsKey(type) ? SortingOrdersDict[type] : 0;

    public static WaitForSeconds GetCachedWaitForSeconds(float time) 
        => WaitForSecDict.ContainsKey(time) ? WaitForSecDict[time] : WaitForSecDict[time] = new WaitForSeconds(time);
}