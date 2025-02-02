using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : SingletonBase<WeaponManager>
{
    private List<Weapon_SO> _weaponInventory = new();

    public void CraftWeapon(string key)
    {        
        Weapon_SO weapon = ResourceManager.Instance.Load<Weapon_SO>(key);
        _weaponInventory.Add(weapon.Clone() as Weapon_SO);
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

    protected override void InitChild()
    {
        _isDontDestroy = false;
    }
}
