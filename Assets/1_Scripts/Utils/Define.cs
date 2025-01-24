public static class Define
{
    public enum EntityType
    {
        Survivor,
        Zombie,
        Barricate,
    }

    public enum WeaponType
    {
        Gun,
        Grenade,
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
    }

    public enum ZombieKeys
    {
        ZOMBIE_NORMAL_01,
        ZOMBIE_NORMAL_02,
    }

    public enum SceneType
    {
        Unknown,
        Main,
        Loading,
        Game,
    }
}
