using Data;
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
    public ZombieStatusByData ZombieStatusByData { get; private set; }
    public Entity Target { get; private set; }
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

    public override void SetupByData(CommonEntityData data)
    {
        base.SetupByData(data);
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
    public void SetAnimatorControllerByKey(string animControllerKey)
        => Animator.runtimeAnimatorController = ResourceManager.Instance.Load<RuntimeAnimatorController>(animControllerKey);

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
        ZombieStatusByData = _status as ZombieStatusByData;
        if (ZombieStatusByData == null)
            DebugUtility.LogError($"[Zombie] Status 타입이 ZombieStatusByData 가 아닙니다.");
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
        => new ZombieStatusByData();

    private void AnimationHashInitialize()
    {
        WalkParamHash = Animator.StringToHash(_walkParamName);
        AttackParamHash = Animator.StringToHash(_attackParamName);
        DieParamHash = Animator.StringToHash(_dieParamName);
    }
}
