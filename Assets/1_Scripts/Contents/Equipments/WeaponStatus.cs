using UnityEngine;

public class WeaponStatus
{
    public string DisplayName {  get; private set; }

    public float Damage { get; private set; }
    public float FireRate { get; private set; }
    public float FireRange { get; private set; }
    public int MaxMagazine { get; private set; }
    public int Magazine { get; private set; }

    public Sprite ProfileSprite { get; private set; }
    public Vector3 WeaponPosition { get; private set; }
    public Vector3 BulletShellPosition {  get; private set; }
    public RuntimeAnimatorController AnimController { get; private set; }

    public void Setup(Weapon_SO so)
    {
        DisplayName = so.DisplayName;
        // To Do - 얘도 SO 수정해야 됨.
        Damage = so.DamageSO.Value;
        FireRate = so.FireRateSO.Value;
        FireRange = so.FireRangeSO.Value;
        MaxMagazine = (int)so.MagazineSO.Value;
        Magazine = (int)so.MagazineSO.Value;

        ProfileSprite = so.ProfileSprite;
        WeaponPosition = so.WeaponPosition;
        BulletShellPosition = so.BulletShellPosition;
        AnimController = so.AnimController;
    }

    public void Fire()
    {
        if (Magazine <= 0)
            return;
        --Magazine;
    }

    public void Reload()
        => Magazine = MaxMagazine;
}
