using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Entity
{
    private Zombie_SO _currentZombieSO;    
    private Queue<Entity> _targetQueue = new();

    private WaitForSeconds _attackDelayTime;
    private Coroutine _attackCoroutine;
    private Entity _currentTarget;

    private const string TARGET_TAG = "HumanSideEntity";

    public override void SetupEntity<T>(string key)
    {
        base.SetupEntity<T>(key);
        _currentZombieSO = CurrentEntitySO as Zombie_SO;
        _currentZombieSO.ResetStats();
        _animator.runtimeAnimatorController = _currentZombieSO.AnimController;
        _attackDelayTime = Util.GetCachedWaitForSeconds(_currentZombieSO.AtkRate.Value);
    }

    private void Update()
    {
        if(_currentZombieSO == null)
        {
            Debug.LogError("현재 Zombie_SO 가 없습니다.");
            return;
        }
        if (_targetQueue.Count > 0)
            return;

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
        transform.position += Vector3.left * _currentZombieSO.MoveSpeed.Value * Time.deltaTime;
    }

    private void Attack()
    {
        _currentTarget = FindNextTarget();
        if(_currentTarget == null)
        {
            Debug.Log($"{GetType().Name}::Target이 없습니다.");
            return;
        }
        Debug.Log($"{GetType().Name}:: Attack");
        _attackCoroutine = StartCoroutine(Co_Attack(_currentTarget));
    }

    private Entity FindNextTarget()
    {
        if(_targetQueue.Count <= 0)
            return null;

        return _targetQueue.Peek();
    }

    private IEnumerator Co_Attack(Entity target)
    {
        target.OnDead += (target) =>
        {
            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
                Attack();
            }
        };

        while (true)
        {
            float damage = _currentZombieSO.Atk.Value;
            target.GetDamaged(this, damage);
            Debug.Log($"{GetType().Name}:: {target.name}에게 {damage}데미지를 입힘");

            yield return _attackDelayTime;
        }        
    }

    protected override void Dispose()
    {
        _currentZombieSO = null;
        _targetQueue.Clear();
        ZombieManager.Instance.Dispawn(this);
    }
}
