using System;
using System.Collections.Generic;

public class WeaponManager : SingletonBase<WeaponManager>
{
    #region Events
    public event Action<WeaponStatus> OnWeaponCreated;
    #endregion

    private List<WeaponStatus> _weaponInventory = new();

    public WeaponStatus CraftWeapon(Weapon_SO originWeaponData)
    {
        WeaponStatus weapon = new WeaponStatus();
        weapon.Setup(originWeaponData);

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

    public void DisassembleWeapon(WeaponStatus weapon)
    {
        _weaponInventory.Remove(weapon);
    }

    public List<WeaponStatus> GetWeaponsList()
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
