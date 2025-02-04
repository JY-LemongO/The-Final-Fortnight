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

    public enum SurvivorKeys
    {
        SURVIVOR_SOLDIER_01,
        SURVIVOR_SOLDIER_02,
        SURVIVOR_SOLDIER_03,
    }

    public enum WeaponKeys
    {
        WEAPON_PISTOL_01,
        WEAPON_SMG_01,
        WEAPON_AR_01,
        Count,
    }

    public enum ZombieKeys
    {
        ZOMBIE_NORMAL_01,
        ZOMBIE_NORMAL_02,
    }
}
