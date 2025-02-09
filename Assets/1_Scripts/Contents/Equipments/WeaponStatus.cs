using UnityEngine;

public class WeaponStatus
{
    public float Damage { get; private set; }
    public float FireRate { get; private set; }
    public float FireRange { get; private set; }
    public int MaxMagazine { get; private set; }
    public int Magazine { get; private set; }

    public Vector3 BulletShellPosition {  get; private set; }

    public void Setup(Weapon_SO so)
    {
        // To Do - 얘도 SO 수정해야 됨.
        Damage = so.Damage.Value;
        FireRange = so.FireRate.Value;
        FireRange = so.FireRange.Value;
        MaxMagazine = (int)so.Magazine.Value;
        Magazine = (int)so.Magazine.Value;

        BulletShellPosition = so.BulletShellPosition;
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
