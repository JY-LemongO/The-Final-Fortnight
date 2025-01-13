using System.Collections.Generic;
using UnityEngine;

public class Zombie : Entity
{
    private Zombie_SO _currentZombieSO;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public override void SetupStats(string key)
    {
        base.SetupStats(key);
        _currentZombieSO = _currentEntitySO as Zombie_SO;
        _animator.runtimeAnimatorController = _currentZombieSO.AnimController;
    }

    private void Update()
    {
        if(_currentZombieSO == null)
        {
            Debug.LogError("현재 Zombie_SO 가 없습니다.");
            return;
        }

        transform.position += Vector3.left * _currentZombieSO.MoveSpeedSO.Value * Time.deltaTime;
    }
}
