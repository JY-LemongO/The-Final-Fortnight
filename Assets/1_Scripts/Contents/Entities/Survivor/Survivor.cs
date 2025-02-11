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
                Controller.SearchTarget();
        }
    }
    private Zombie _target;

    protected override void Init()
    {
        base.Init();
        EntityType = Define.EntityType.Survivor;
        SurvivorStatus = _status as SurvivorStatus;
        if (SurvivorStatus == null)
            DebugUtility.LogError($"[Survivor] Status 타입이 SurvivorStatus가 아닙니다.");
        AnimationHashInitialize();
    }

    public void SetWeapon(WeaponStatus weapon)
    {
        if (Weapon == null)
        {
            GameObject go = ResourceManager.Instance.Instantiate(Constants.Key_Weapon);
            go.transform.SetParent(transform);
            Weapon = go.GetComponent<Weapon>();
        }            

        Weapon.SetupWeapon(this, weapon);
        Weapon.transform.localPosition = weapon.WeaponPosition;
        Controller.SearchTarget();
    }

    public void SetBulletUI()
    {
        UI_Bullet ui = UIManager.Instance.CreateWorldUI<UI_Bullet>();
        ui.SetSurvivor(this);
        ui.transform.position = transform.position + Vector3.up * 1.2f;
    }

    public void SetTarget(Zombie zombie)
        => Target = zombie;

    private void AnimationHashInitialize()
    {
        _walkParamHash = Animator.StringToHash(_walkParamName);
        _attackParamHash = Animator.StringToHash(_attackParamName);
        _dieParamHash = Animator.StringToHash(_dieParamName);
    }

    protected override void ComponenetsSetting()
    {
        base.ComponenetsSetting();
        Animator = GetComponent<Animator>();
        Controller = GetComponent<SurvivorController>();
        Controller.Setup(this);
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
    
}
