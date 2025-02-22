using UnityEngine;

public class Zombie : Entity, IAnimatedObject
{
    #region Animation
    [Header("AnimationData")]
    [SerializeField] private string _walkParamName;
    [SerializeField] private string _attackParamName;
    [SerializeField] private string _dieParamName;    

    public int WalkParamHash { get; private set; }
    public int AttackParamHash { get; private set; }
    public int DieParamHash { get; private set; }
    #endregion    

    public Animator Animator { get; private set; }
    public ZombieController ZombieController { get; private set; }
    public ZombieStatus ZombieStatus { get; private set; }
    public Entity Target {  get; private set; }
    public float SpriteHalfSize { get; private set; }

    private void Update()
    {
        if (Target == null)
            ZombieController.SearchTargetByRay();
    }

    public override void Setup(Entity_SO so)
    {
        base.Setup(so);
        ZombieController.Move();
    }

    public void SetTarget(Entity target)
        => Target = target;

    public override void GetDamaged(float damage)
    {
        base.GetDamaged(damage);
        DebugUtility.Log($"[Zombie] GetDamaged 오버라이드 함수 내부");
    }

    public void SetAnimatorController(RuntimeAnimatorController controller)
        => Animator.runtimeAnimatorController = controller;

    public override void ResetEntity()
    {
        StopAllCoroutines();
        Target = null;
        PoolManager.Instance.Return(gameObject);
    }

    protected override void Init()
    {
        base.Init();
        EntityType = Define.EntityType.Zombie;
        ZombieStatus = _status as ZombieStatus;
        SpriteHalfSize = _renderer.sprite.textureRect.height / _renderer.sprite.pixelsPerUnit * 0.5f;        
        AnimationHashInitialize();
    }

    protected override void ComponentsSetting()
    {
        base.ComponentsSetting();
        ZombieController = GetComponent<ZombieController>();
        Animator = GetComponent<Animator>();

        ZombieController.Setup(this);
    }

    protected override EntityStatus CreateStatusInstance()
        => new ZombieStatus();        

    private void AnimationHashInitialize()
    {
        WalkParamHash = Animator.StringToHash(_walkParamName);
        AttackParamHash = Animator.StringToHash(_attackParamName);
        DieParamHash = Animator.StringToHash(_dieParamName);
    }    
}
