using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/New Weapon", fileName = "WEAPON_")]
public class Weapon_SO : BaseSO
{
    [Header("Weapon Stats")]
    [SerializeField] private float _damage;    
    [SerializeField] private float _fireRate;
    [SerializeField] private float _fireRange;
    [SerializeField] private int _magazine;
    [Header("Visual")]
    [SerializeField] private RuntimeAnimatorController _animController;
    [SerializeField] private Sprite _profileSprite;
    [SerializeField] private Vector2 _weaponPosition;
    [SerializeField] private Vector2 _bulletShellPosition;
        
    public float Damage => _damage;    
    public float FireRate => _fireRate;
    public float FireRange => _fireRange;
    public int Magazine => _magazine;    

    public RuntimeAnimatorController AnimController => _animController;
    public Sprite ProfileSprite => _profileSprite;
    public Vector3 WeaponPosition => _weaponPosition;
    public Vector3 BulletShellPosition => _bulletShellPosition;
}
