public static class Define
{
    public enum SceneType
    {
        Unknown,
        Main,
        Loading,
        Game,
    }

    public enum UIType
    {
        Scene,
        Popup,
        World,
        Item,
    }

    public enum EntityType
    {
        Survivor,
        Zombie,
        Structure,
        MainBarricate,
    }

    public enum WeaponType
    {
        Gun,
        Grenade,
    }

    public enum StructureType
    {
        Barricate,
        Turret,
    }

    public enum BarricateTier
    {
        BARRICATE_LV1,
        BARRICATE_LV2,
    }

    public enum SpriteType
    {
        Background,
        Foreground,
        Entity,
        Weapon,
        WorldUI,
    }
}
