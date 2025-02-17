using System;
using System.Collections.Generic;

public class WeaponManager : SingletonBase<WeaponManager>
{
    #region Events
    public event Action<WeaponStatus> OnWeaponCreated;
    public event Action<WeaponStatus, bool> OnEquipmentChanged;
    #endregion

    private Dictionary<WeaponStatus, Survivor> _equippedWeaponDict = new();
    private List<WeaponStatus> _weaponInventory = new();

    public WeaponStatus CraftWeapon(Weapon_SO originWeaponData)
    {
        WeaponStatus weapon = new WeaponStatus(originWeaponData);

        _weaponInventory.Add(weapon);
        OnWeaponCreated?.Invoke(weapon);

        return weapon;
    }

    public void RegisterWeapon(Survivor survivor, WeaponStatus weapon)
    {

    }

    public void EquipWeapon(Survivor survivor, WeaponStatus weapon)
    {
        // 이미 장착중이던 무기일 경우 A, B 무기 스왑
        if (IsEquippedWeapon(weapon))
            SwapWeapon(survivor, weapon);
        else
        {
            if (survivor.Weapon.WeaponStatus != null)
                UnequipWeapon(survivor);

            _equippedWeaponDict[weapon] = survivor;
            survivor.SetWeapon(weapon);
            OnEquipmentChanged?.Invoke(weapon, true);
        }        
    }

    private void UnequipWeapon(Survivor survivor)
    {
        WeaponStatus prevWeapon = survivor.Weapon.WeaponStatus;
        _equippedWeaponDict.Remove(prevWeapon);
        OnEquipmentChanged?.Invoke(prevWeapon, false);
    }        

    private void SwapWeapon(Survivor survivor, WeaponStatus weapon)
    {
        Survivor weaponOwner = _equippedWeaponDict[weapon];
        WeaponStatus survivorsWeapon = survivor.Weapon.WeaponStatus;
        weaponOwner.SetWeapon(survivorsWeapon);
        survivor.SetWeapon(weapon);

        _equippedWeaponDict[weapon] = survivor;
        _equippedWeaponDict[survivorsWeapon] = weaponOwner;
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

    public int GetWeaponsCount()
        => _weaponInventory.Count;

    public bool IsEquippedWeapon(WeaponStatus weapon)
    {
        foreach (var equippedWeapon in _equippedWeaponDict.Keys)
            if (weapon == equippedWeapon)
                return true;

        return false;
    }

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
