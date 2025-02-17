using System;
using UnityEngine;

public class Weapon : MonoBehaviour, IAnimatedObject
{
    #region Events
    public event Action<int, int> OnMagazineValueChanged;
    public event Action<bool> OnReload;
    #endregion

    #region Animation
    [Header("AnimationData")]
    [SerializeField] private string _fireParamName;
    [SerializeField] private string _reloadParamName;

    public int FireParamHash { get; private set; }
    public int ReloadParamHash { get; private set; }
    #endregion

    public Animator Animator { get; private set; }
    public WeaponStatus WeaponStatus { get; private set; }
    public SpriteRenderer Renderer { get; private set; }
    public Zombie CurrentTarget;

    public int CurrentMagazine
    {
        get => _currentMagazine;
        private set
        {
            _currentMagazine = value;
            OnMagazineValueChanged?.Invoke(_currentMagazine, WeaponStatus.MaxMagazine);

            if (_currentMagazine == 0)
                Reload();
        }
    }
    private int _currentMagazine;

    private void Awake()
    {
        Init();
    }

    public void Fire(Zombie target)
    {
        CurrentTarget = target;
        Animator.SetTrigger(FireParamHash);
    }

    public void Reload()
    {
        Animator.SetTrigger(ReloadParamHash);
        OnReload?.Invoke(true);
    }      

    public void SetupWeapon(WeaponStatus newWeapon)
    {
        WeaponStatus = newWeapon;
        CurrentMagazine = newWeapon.MaxMagazine;

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
        Renderer = GetComponent<SpriteRenderer>();        
    }

    private void AnimationHashInitialize()
    {
        FireParamHash = Animator.StringToHash(_fireParamName);
        ReloadParamHash = Animator.StringToHash(_reloadParamName);
    }

    private void SpawnBulletShell()
    {
        GameObject go = ResourceManager.Instance.Instantiate(Constants.Key_BulletShell);
        BulletShell shell = go.GetComponent<BulletShell>();
        Vector3 shellPos = transform.position + WeaponStatus.BulletShellPosition;
        shell.Setup(shellPos);
    }

    #region Animation Triggers
    private void HandleAttackTarget()
    {
        DebugUtility.Log($"[WeaponController] 좀비 공격 - {CurrentTarget.name}");

        CurrentMagazine--;
        CurrentTarget.GetDamaged(WeaponStatus.Damage);
        SpawnBulletShell();
    }

    private void HandleReload()
    {
        CurrentMagazine = WeaponStatus.MaxMagazine;
        OnReload?.Invoke(false);
    }
    #endregion
}
