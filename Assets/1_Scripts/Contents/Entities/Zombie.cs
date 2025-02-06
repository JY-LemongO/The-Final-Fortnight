using System.Collections;
using UnityEngine;

public class Zombie : Entity
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
    
    private Zombie_SO _currentZombieSO => CurrentEntitySO as Zombie_SO;
    private Entity _currentTarget;
    private float _spriteHalfSize;

    private void Update()
    {
        if (_currentTarget == null)
            SearchTargetByRay();
    }

    protected override void Init()
    {
        base.Init();
        EntityType = Define.EntityType.Zombie;        
    }

    public override void SetupEntity<T>(T entityClone)
    {
        if(entityClone is not Zombie_SO)
        {
            Debug.LogError($"SetupEntity: 잘못된 Entity_SO 타입입니다. {typeof(T)}");
            return;
        }

        base.SetupEntity(entityClone);        
        _currentZombieSO.ResetStats();
        _anim.runtimeAnimatorController = _currentZombieSO.AnimController;
        _spriteHalfSize = _renderer.sprite.textureRect.height / _renderer.sprite.pixelsPerUnit * 0.5f;

        Move();
    }

    private void Move()
        => StartCoroutine(Co_Move());

    private void Attack()
        => _anim.SetBool(_attackParamHash, true);

    private void SearchTargetByRay()
    {
        Vector2 origin = (Vector2)transform.position + Vector2.down * _spriteHalfSize;
        Vector2 direction = Vector2.left;
        float distance = _currentZombieSO.Range.Value;

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

    #region AnimationEventTrigger
    private void HandleAttackTarget()
    {
        Debug.Log("Zombie가 공격");
        float damage = _currentZombieSO.Atk.Value;                
        _currentTarget.GetDamaged(this, damage);
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
        Dispose();
        ZombieManager.Instance.Dispawn(this);
    }        
    #endregion

    private IEnumerator Co_Move()
    {
        _anim.SetBool(_attackParamHash, false);
        _anim.SetBool(_walkParamHash, true);
        while (true)
        {
            if (IsDead) break;
            if (_currentTarget != null) break;

            transform.position += Vector3.left * _currentZombieSO.MoveSpeed.Value * Time.deltaTime;            
            yield return null;
        }
    }

    protected override void Dead()
    {
        _anim.SetTrigger(_dieParamHash);
        base.Dead();
    }

    protected override void AnimationHashInitialize()
    {
        _walkParamHash = Animator.StringToHash(_walkParamName);
        _attackParamHash = Animator.StringToHash(_attackParamName);
        _dieParamHash = Animator.StringToHash(_dieParamName);
    }

    public override void ResetEntity()
    {
        base.ResetEntity();
        Dispose();
    }

    public override void Dispose()
    {
        StopAllCoroutines();
        _renderer.color = Color.white;        
        _currentTarget = null;        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 origin = (Vector2)transform.position + Vector2.down * _spriteHalfSize;        
        Gizmos.DrawRay(origin, Vector3.left * _currentZombieSO.Range.Value);
    }
}
