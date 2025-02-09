using System.Collections;
using UnityEngine;

public class Zombie : Entity, IAnimatedObject
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

    [SerializeField] private LayerMask _targetLayer;

    public Animator Animator { get; private set; }
    public ZombieStatus ZombieStatus { get; private set; }

    private Entity _currentTarget;
    private float _spriteHalfSize;

    private void Update()
    {
        if (_currentTarget == null)
            SearchTargetByRay();
    }    

    public void SetAnimatorController(RuntimeAnimatorController controller)
        => Animator.runtimeAnimatorController = controller;

    public override void ResetEntity()
    {
        StopAllCoroutines();
        _renderer.color = Color.white;
        _currentTarget = null;
    }

    public override void GetDamaged(float damage)
    {
        DebugUtility.Log($"[Zombie] GetDamaged 오버라이드 함수 내부");
    }

    protected override void Init()
    {
        base.Init();
        EntityType = Define.EntityType.Zombie;
        ZombieStatus = _status as ZombieStatus;
        AnimationHashInitialize();
    }

    protected override void ComponenetsSetting()
    {
        base.ComponenetsSetting();
        Animator = GetComponent<Animator>();
    }

    protected override EntityStatus CreateStatusInstance()
        => new ZombieStatus();    

    private void Move()
        => StartCoroutine(Co_Move());

    private void Attack()
        => Animator.SetBool(_attackParamHash, true);

    private void SearchTargetByRay()
    {
        Vector2 origin = (Vector2)transform.position + Vector2.down * _spriteHalfSize;
        Vector2 direction = Vector2.left;
        float distance = ZombieStatus.Range;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, _targetLayer);
        if (hit)
        {
            Entity target = hit.transform.GetComponent<Entity>();
            if (target.IsDead)
                return;

            _currentTarget = target;
            Attack();
        }            
    }

    private void AnimationHashInitialize()
    {
        _walkParamHash = Animator.StringToHash(_walkParamName);
        _attackParamHash = Animator.StringToHash(_attackParamName);
        _dieParamHash = Animator.StringToHash(_dieParamName);
    }

    #region AnimationEventTrigger
    private void HandleAttackTarget()
    {
        Debug.Log("Zombie가 공격");
        float damage = ZombieStatus.Attack;                
        _currentTarget.GetDamaged(damage);
    }

    private void HandleFindTarget()
    {         
        if (_currentTarget.IsDead)
        {
            _currentTarget = null;
            Move();
        }            
    }

    private void HandleDie()
    {        
        ZombieManager.Instance.Dispawn(this);
    }        
    #endregion

    private IEnumerator Co_Move()
    {
        Animator.SetBool(_attackParamHash, false);
        Animator.SetBool(_walkParamHash, true);
        while (true)
        {
            if (IsDead) break;
            if (_currentTarget != null) break;

            transform.position += Vector3.left * ZombieStatus.MoveSpeed * Time.deltaTime;            
            yield return null;
        }
    }
}
