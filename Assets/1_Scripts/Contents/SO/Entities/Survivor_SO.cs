using UnityEngine;

[CreateAssetMenu(menuName = "Entity/New Survivor", fileName = "SURVIVOR_")]
public class Survivor_SO : Entity_SO
{
    [Header("Survivor Stats")]
    [SerializeField] private Weapon_SO _defaultWeaponSO;
    [Header("Visual")]    
    [SerializeField] private Sprite _profileSprite;

    public Weapon_SO DefaultWeapon => _defaultWeaponSO;    
    public Sprite ProfileSprite => _profileSprite;
}
