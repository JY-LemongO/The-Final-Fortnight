using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Entity
{
    private Queue<Entity> _targetQueue = new();
    private Zombie_SO _currentZombieSO;    
    private Entity _currentTarget;    

    private const string TARGET_TAG = "HumanSideEntity";

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
        if (!collision.CompareTag(TARGET_TAG))
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
        _anim.SetBool(_attackParamHash, true);
        _currentTarget = FindNextTarget();
    }

    private Entity FindNextTarget()
    {
        if (_targetQueue.Count <= 0)
            return null;

        return _targetQueue.Dequeue();
    }

    #region AnimationEventTrigger
    private void HandleAttackTarget()
    {
        float damage = _currentZombieSO.Atk.Value;
        _currentTarget = FindNextTarget();
        if (_currentTarget == null)
        {
            Debug.Log($"{GetType().Name}::Target이 없습니다.");
            return;
        }
        _currentTarget.GetDamaged(this, damage);
    }

    private void HandleFindTarget()
    {
        if (_currentTarget.IsDead)
            _currentTarget = FindNextTarget();
        if (_currentTarget == null)
            Move();
    }

    private void HandleDie()
    {
        Dispose();
    }
    #endregion

    private IEnumerator Co_Move()
    {
        _anim.SetBool(_attackParamHash, false);
        _anim.SetBool(_walkParamHash, true);
        while (_currentTarget == null)
        {
            transform.position += Vector3.left * _currentZombieSO.MoveSpeed.Value * Time.deltaTime;
            if(IsDead) break;
            yield return null;
        }
    }

    protected override void Dispose()
    {
        _renderer.color = Color.white;
        _currentZombieSO = null;
        _targetQueue.Clear();
        ZombieManager.Instance.Dispawn(this);
    }
}
