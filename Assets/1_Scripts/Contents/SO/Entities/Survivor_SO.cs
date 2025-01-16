using UnityEngine;

[CreateAssetMenu(menuName = "Entitity/New Survivor", fileName = "SURVIVOR_")]
public class Survivor_SO : Entity_SO
{
    [Header("Survivor Stats")]
    [SerializeField] private Weapon_SO _defaultWeaponSO;
    [Header("Visual")]
    [SerializeField] private RuntimeAnimatorController _animController;

    public Weapon_SO DefaultWeapon => _defaultWeaponSO;
    public RuntimeAnimatorController AnimController => _animController;

    public override void InitializeStats()
    {
        base.InitializeStats();
        _defaultWeaponSO.Clone();        
    }

    public override object Clone()
    {
        var survivorClone = Instantiate(this);
        survivorClone.InitializeStats();

        return survivorClone;
    }
}
