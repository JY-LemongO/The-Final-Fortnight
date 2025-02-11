using System;
using UnityEngine;

public class WeaponStatus
{
    public event Action<int, int> OnMagazineValueChanged;
    public event Action OnReload;

    public string DisplayName {  get; private set; }

    public float Damage { get; private set; }
    public float FireRate { get; private set; }
    public float FireRange { get; private set; }
    public int MaxMagazine { get; private set; }
    public int Magazine { get; private set; }

    public bool IsEquiped {  get; private set; }

    public Sprite ProfileSprite { get; private set; }
    public Vector3 WeaponPosition { get; private set; }
    public Vector3 BulletShellPosition {  get; private set; }
    public RuntimeAnimatorController AnimController { get; private set; }

    public void Setup(Weapon_SO so)
    {
        DisplayName = so.DisplayName;
        
        Damage = so.Damage;
        FireRate = so.FireRate;
        FireRange = so.FireRange;
        MaxMagazine = so.Magazine;
        Magazine = so.Magazine;

        ProfileSprite = so.ProfileSprite;
        WeaponPosition = so.WeaponPosition;
        BulletShellPosition = so.BulletShellPosition;
        AnimController = so.AnimController;
    }

    public void UseBullet()
    {        
        OnMagazineValueChanged?.Invoke(--Magazine, MaxMagazine);
        if (Magazine <= 0)
        {
            OnReload?.Invoke();
            return;
        }
    }

    public void Reload()
        => Magazine = MaxMagazine;

    public void Equip()
    {
        IsEquiped = true;
    }

    public void Unequip()
    {
        IsEquiped = false;
    }
}
