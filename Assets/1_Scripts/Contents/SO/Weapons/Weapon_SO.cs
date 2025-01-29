using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/New Weapon", fileName = "WEAPON_")]
public class Weapon_SO : BaseSO
{
    public event Action OnReloadWeapon;

    [SerializeField] private Stat_SO _damageSO;
    [SerializeField] private Stat_SO _magazineSO;
    [SerializeField] private Stat_SO _fireRateSO;
    [SerializeField] private Stat_SO _fireRangeSO;
    [Header("Visual")]
    [SerializeField] private RuntimeAnimatorController _animController;
    [SerializeField] private Sprite _profileSprite;
    [SerializeField] private Vector2 _weaponPosition;
    [SerializeField] private Vector2 _bulletShellPosition;

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
    public Vector3 WeaponPosition => _weaponPosition;
    public Vector3 BulletShellPosition => _bulletShellPosition;

    public void Fire()
    {
        if (Magazine.Value <= 0)
            return;

        Magazine.Consume(1);
    }

    public void Reload()
        => Magazine.ResetCurrentValue();

    public void InitilizeWeaponStat()
    {
        _statDamage = _damageSO.Clone() as Stat_SO;
        _statMagazine = _magazineSO.Clone() as Stat_SO;
        _statFireRate = _fireRateSO.Clone() as Stat_SO;
        _statFireRange = _fireRangeSO.Clone() as Stat_SO;

        _statMagazine.OnStatCurrentValueChanged += (current, total) =>
        {
            if (current == 0)
                OnReloadWeapon?.Invoke();
        };
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
    }
}
