using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region renderer
    [Header("AnimationData")]
    [SerializeField] private string _fireParamName;
    [SerializeField] private string _reloadParamName;    

    private SpriteRenderer _renderer;
    private Animator _anim;
    private int _fireParamHash;
    private int _reloadParamHash;    
    #endregion

    public Weapon_SO WeaponData => _currentWeapon;
    private Weapon_SO _currentWeapon;
    private Survivor _context;

    private float _currentCooldown;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.sortingOrder = Util.GetSortingOreder(Define.SpriteType.Weapon);

        AnimationHashInitialize();
    }

    private void Update()
    {
        if (_context == null)
            return;

        HandleGunFireTimer();
    }

    public void InitializeWeapon(Survivor context, Weapon_SO weapon)
    {
        _context = context;
        _currentWeapon = weapon;
        _anim.runtimeAnimatorController = weapon.AnimController;
    }

    public void ChangeWeapon(Weapon_SO newWeapon)
        => _currentWeapon = newWeapon;

    private void Fire()
        => _anim.SetTrigger(_fireParamHash);
    private void Reload()
        => _anim.SetTrigger(_reloadParamHash);

    private void HandleGunFireTimer()
    {
        if (_currentCooldown < _currentWeapon.FireRate.Value)
            _currentCooldown += Time.deltaTime;
        else
        {
            if (_context.Target == null)
                return;
            if (_currentWeapon.Magazine.CurrentValue <= 0)
                return;
            
            Fire();
            _currentCooldown = 0f;
        }
    }

    #region AnimationEventTrigger
    private void AnimationHashInitialize()
    {
        _fireParamHash = Animator.StringToHash(_fireParamName);
        _reloadParamHash = Animator.StringToHash(_reloadParamName);        
    }

    private void HandleAttackTarget()
    {
        if (!_context.CheckTargetIsDead())
        {
            _currentWeapon.Fire();
            _context.AttackTarget(_currentWeapon.Damage.Value);
            Debug.Log($"남은 탄약 [{_currentWeapon.Magazine.CurrentValue}/{_currentWeapon.Magazine.Value}]");
        }            
    }

    private void HandleCheckMagazineIsEmpty()
    {
        if (_currentWeapon.Magazine.CurrentValue <= 0)
            Reload();
    }

    private void HandleReload()
    {
        _currentWeapon.Magazine.ResetCurrentValue();
    }
    #endregion
}
