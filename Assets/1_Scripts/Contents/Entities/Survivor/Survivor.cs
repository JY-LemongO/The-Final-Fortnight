using UnityEngine;

public class Survivor : Entity, IAnimatedObject
{
    #region Renderer
    [Header("AnimationData")]
    [SerializeField] private string _walkParamName;
    [SerializeField] private string _attackParamName;
    [SerializeField] private string _dieParamName;

    protected int _walkParamHash;
    protected int _attackParamHash;
    protected int _dieParamHash;
    #endregion

    public Animator Animator { get; private set; }
    public SurvivorController Controller { get; private set; }
    public SurvivorStatus SurvivorStatus { get; private set; }
    public Weapon Weapon { get; private set; }
    public Zombie Target
    {
        get => _target;
        private set
        {
            _target = value;
            if (_target == null)
                Controller.SearchTarget(Weapon.WeaponStatus.FireRange);
        }
    }
    private Zombie _target;
    private UI_Bullet _bulletUI;
    private float _weaponCooldownTime;
    private bool _isReloading = false;

    private void Update()
    {
        HandleUseWeapon();
    }

    protected override void Init()
    {
        base.Init();
        EntityType = Define.EntityType.Survivor;
        SurvivorStatus = _status as SurvivorStatus;
        if (SurvivorStatus == null)
            DebugUtility.LogError($"[Survivor] Status 타입이 SurvivorStatus가 아닙니다.");

        InitWeapon();
        AnimationHashInitialize();
    }

    public void InitWeapon()
    {
        if (Weapon != null)
        {
            DebugUtility.LogWarning("[Survivor] 해당 생존자는 이미 무기 프리팹을 가지고 있습니다.");
            return;
        }

        GameObject go = ResourceManager.Instance.Instantiate(Constants.Key_Weapon);
        go.transform.SetParent(transform);
        Weapon = go.GetComponent<Weapon>();
        Weapon.OnReload += (reloading) =>
        {
            _isReloading = reloading;
            if (_isReloading)
                _weaponCooldownTime = 0f;
        };
        SetBulletUI();
    }

    public void SetWeapon(WeaponStatus weapon)
    {
        Weapon.SetupWeapon(weapon);
        Weapon.transform.localPosition = weapon.WeaponPosition;
        Weapon.Renderer.sortingOrder = _renderer.sortingOrder + 1;

        _bulletUI.SetupBulletUI();
        Controller.SearchTarget(Weapon.WeaponStatus.FireRange);
    }

    public void SetBulletUI()
    {
        _bulletUI = UIManager.Instance.CreateWorldUI<UI_Bullet>();
        _bulletUI.SetSurvivor(this);
        _bulletUI.transform.position = transform.position + Vector3.up * 1.2f;
    }

    public void SetTarget(Zombie zombie)
        => Target = zombie;

    private void AnimationHashInitialize()
    {
        _walkParamHash = Animator.StringToHash(_walkParamName);
        _attackParamHash = Animator.StringToHash(_attackParamName);
        _dieParamHash = Animator.StringToHash(_dieParamName);
    }

    protected override void ComponentsSetting()
    {
        base.ComponentsSetting();
        Animator = GetComponent<Animator>();
        Controller = GetComponent<SurvivorController>();        
    }

    public override void ResetEntity()
    {
        StopAllCoroutines();
    }

    public override void GetDamaged(float damage)
    {
        base.GetDamaged(damage);
        DebugUtility.Log($"[Survivor] GetDamaged 오버라이드 함수 내부");
    }

    public void SetAnimatorController(RuntimeAnimatorController controller)
        => Animator.runtimeAnimatorController = controller;

    protected override EntityStatus CreateStatusInstance()
        => new SurvivorStatus();

    #region WeaponLogic
    private void HandleUseWeapon()
    {
        if (_isReloading)
            return;

        _weaponCooldownTime += Time.deltaTime;

        if (Weapon == null)
        {
            DebugUtility.LogError("[Survivor] Weapon is null.");
            return;
        }
        if (Weapon.WeaponStatus == null || Target == null)
            return;
        
        if(_weaponCooldownTime >= Weapon.WeaponStatus.FireRate)
        {
            if (Target.Status.IsDead)
                Target = null;

            _weaponCooldownTime = 0f;
            Weapon.Fire(Target);
        }
    }
    #endregion
}
