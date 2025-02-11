using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/New Weapon", fileName = "WEAPON_")]
public class Weapon_SO : BaseSO
{
    [SerializeField] private Stat_SO _damageSO;
    [SerializeField] private Stat_SO _magazineSO;
    [SerializeField] private Stat_SO _fireRateSO;
    [SerializeField] private Stat_SO _fireRangeSO;
    [Header("Visual")]
    [SerializeField] private RuntimeAnimatorController _animController;
    [SerializeField] private Sprite _profileSprite;
    [SerializeField] private Vector2 _weaponPosition;
    [SerializeField] private Vector2 _bulletShellPosition;
    
    #region Read Only Property
    public Stat_SO DamageSO => _damageSO;
    public Stat_SO MagazineSO => _magazineSO;
    public Stat_SO FireRateSO => _fireRateSO;
    public Stat_SO FireRangeSO => _fireRangeSO;
    #endregion

    public RuntimeAnimatorController AnimController => _animController;
    public Sprite ProfileSprite => _profileSprite;
    public Vector3 WeaponPosition => _weaponPosition;
    public Vector3 BulletShellPosition => _bulletShellPosition;
}
