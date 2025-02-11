using System;
using UnityEngine;

public class Weapon : MonoBehaviour, IAnimatedObject
{
    #region Animation
    [Header("AnimationData")]
    [SerializeField] private string _fireParamName;
    [SerializeField] private string _reloadParamName;

    public int FireParamHash { get; private set; }
    public int ReloadParamHash { get; private set; }
    #endregion

    public Animator Animator { get; private set; }
    public WeaponStatus WeaponStatus { get; private set; }
    public WeaponController WeaponController { get; private set; }
    public Survivor WeaponOwner { get; private set; }
    public SpriteRenderer Renderer { get; private set; }

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (WeaponOwner == null)
        {
            DebugUtility.LogError("[Weapon] WeaponOwner가 없습니다.");
            return;
        }
        WeaponController.HandleGunFireTimer();
    }

    public void Fire()
        => Animator.SetTrigger(FireParamHash);

    public void Reload()
        => Animator.SetTrigger(ReloadParamHash);

    public void SetupWeapon(Survivor owner, WeaponStatus weapon)
    {
        WeaponOwner = owner;
        ChangeWeapon(weapon);        
    }

    public void ChangeWeapon(WeaponStatus newWeapon)
    {
        if (WeaponStatus != null)
        {
            WeaponStatus.OnReload -= Reload;
            WeaponStatus.Unequip();
        }            
        WeaponStatus = newWeapon;
        WeaponStatus.OnReload += Reload;
        WeaponStatus.Equip();

        transform.position = newWeapon.WeaponPosition;
        SetAnimatorController(newWeapon.AnimController);
    }

    public void SetAnimatorController(RuntimeAnimatorController controller)
        => Animator.runtimeAnimatorController = controller;

    private void Init()
    {
        ComponentsSetting();
        AnimationHashInitialize();
    }

    private void ComponentsSetting()
    {
        Animator = GetComponent<Animator>();
        WeaponController = GetComponent<WeaponController>();
        WeaponController.Setup(this);
        Renderer = GetComponent<SpriteRenderer>();        
        Renderer.sortingOrder = Util.GetSortingOreder(Define.SpriteType.Weapon);
    }

    private void AnimationHashInitialize()
    {
        FireParamHash = Animator.StringToHash(_fireParamName);
        ReloadParamHash = Animator.StringToHash(_reloadParamName);
    }
}
