using System;
using System.Collections.Generic;

public class WeaponManager : SingletonBase<WeaponManager>
{
    #region Events
    public event Action<Weapon_SO> OnWeaponCreated;
    #endregion

    private List<Weapon_SO> _weaponInventory = new();

    public Weapon_SO CraftWeapon(Weapon_SO originWeaponData)
    {
        Weapon_SO weapon = originWeaponData.Clone() as Weapon_SO;
        _weaponInventory.Add(weapon);
        OnWeaponCreated?.Invoke(weapon);

        return weapon;
    }

    public void SortInventoryByEquipment()
    {
        // To Do - 장착 중인 무기 순으로 정렬, 그 이후는 희귀도
    }

    public void SortInventoryByRarity()
    {
        // To Do - 장착 여부에 상관없이 희귀도 순으로 정렬
    }

    public void DisassembleWeapon(Weapon_SO weapon)
    {
        _weaponInventory.Remove(weapon);
    }

    public List<Weapon_SO> GetAllWeapons()
        => _weaponInventory;

    protected override void InitChild()
    {
        _isDontDestroy = false;
    }

    public override void Dispose()
    {
        _weaponInventory.Clear();
        OnWeaponCreated = null;        
        base.Dispose();
    }
}
