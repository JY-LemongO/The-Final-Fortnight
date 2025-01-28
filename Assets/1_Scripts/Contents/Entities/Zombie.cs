using System.Collections;
using System.Collections.Generic;
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

    private Queue<Entity> _targetQueue = new();
    private Zombie_SO _currentZombieSO;    
    private Entity _currentTarget;    

    protected override void Init()
    {
        base.Init();
        EntityType = Define.EntityType.Zombie;
    }

    public override void SetupEntity<T>(string key)
    {
        base.SetupEntity<T>(key);
        _currentZombieSO = CurrentEntitySO as Zombie_SO;
        _currentZombieSO.ResetStats();
        _anim.runtimeAnimatorController = _currentZombieSO.AnimController;        

        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(Constants.TARGET_TAG))
            return;

        _targetQueue.Enqueue(collision.GetComponent<Entity>());

        if (_currentTarget == null)
            Attack();
    }

    private void Move()
    {
        StartCoroutine(Co_Move());        
    }

    private void Attack()
    {
        _currentTarget = FindNextTarget();
        _anim.SetBool(_attackParamHash, true);        
    }

    private Entity FindNextTarget()
    {
        if (_targetQueue.Count <= 0)
            return null;

        return _targetQueue.Peek();
    }

    #region AnimationEventTrigger
    private void HandleAttackTarget()
    {
        float damage = _currentZombieSO.Atk.Value;                
        _currentTarget.GetDamaged(this, damage);
    }

    private void HandleFindTarget()
    {
        if (_currentTarget.IsDead)
        {
            _targetQueue.Dequeue();
            _currentTarget = FindNextTarget();
        }            
        if (_currentTarget == null)
            Move();
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
        _currentZombieSO = null;
        _currentTarget = null;
        _targetQueue.Clear();        
    }
}
