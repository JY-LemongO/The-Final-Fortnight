using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/New Weapon", fileName = "WEAPON_")]
public class Weapon_SO : BaseSO
{
    public event Action<int> OnShootWeapon;
    public event Action<int> OnReloadWeapon;

    [SerializeField] private Stat_SO _damageSO;
    [SerializeField] private Stat_SO _magazineSO;
    [SerializeField] private Stat_SO _fireRateSO;
    [SerializeField] private Stat_SO _fireRangeSO;
    [Header("Visual")]
    [SerializeField] private RuntimeAnimatorController _animController;
    [SerializeField] private Sprite _profileSprite;
    [SerializeField] private Vector2 _weaponPosition;

    private Stat_SO _statDamage;
    private Stat_SO _statMagazine;
    private Stat_SO _statFireRate;
    private Stat_SO _statFireRange;

    public Stat_SO Damage => _statDamage;
    public Stat_SO Magazine => _statMagazine;
    public Stat_SO FireRate => _statFireRate;
    public Stat_SO FireRange => _statFireRange;

    public RuntimeAnimatorController AnimController => _animController;
    public Sprite ProfileSprite => _profileSprite;
    public Vector2 WeaponPosition => _weaponPosition;

    public void Fire()
    {
        if (Magazine.Value <= 0)
            return;

        Magazine.Consume(1);
        OnShootWeapon?.Invoke((int)Magazine.CurrentValue);
    }

    public void Reload()
    {
        Magazine.Recover(Magazine.Value);
        OnReloadWeapon?.Invoke((int)Magazine.CurrentValue);
    }

    public void InitilizeWeaponStat()
    {
        _statDamage = _damageSO.Clone() as Stat_SO;
        _statMagazine = _magazineSO.Clone() as Stat_SO;
        _statFireRate = _fireRateSO.Clone() as Stat_SO;
        _statFireRange = _fireRangeSO.Clone() as Stat_SO;
    }

    public override object Clone()
    {
        var weaponClone = Instantiate(this);
        weaponClone.InitilizeWeaponStat();

        return weaponClone;
    }

    public override void Dispose()
    {
        OnReloadWeapon = null;
        OnShootWeapon = null;
    }
}
