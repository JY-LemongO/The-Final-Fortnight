using UnityEngine;

public class WeaponStatus
{
    public string DisplayName {  get; private set; }

    public float Damage { get; private set; }
    public float FireRate { get; private set; }
    public float FireRange { get; private set; }
    public int MaxMagazine { get; private set; }    

    public Sprite ProfileSprite { get; private set; }
    public Vector3 WeaponPosition { get; private set; }
    public Vector3 BulletShellPosition {  get; private set; }
    public RuntimeAnimatorController AnimController { get; private set; }

    public WeaponStatus(Weapon_SO so)
    {
        DisplayName = so.DisplayName;
        
        Damage = so.Damage;
        FireRate = so.FireRate;
        FireRange = so.FireRange;
        MaxMagazine = so.Magazine;        

        ProfileSprite = so.ProfileSprite;
        WeaponPosition = so.WeaponPosition;
        BulletShellPosition = so.BulletShellPosition;
        AnimController = so.AnimController;
    }
}
