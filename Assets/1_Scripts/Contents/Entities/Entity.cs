using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public event Action OnDead;

    protected Dictionary<string, Entity_SO> _entitySODict = new();

    protected Entity_SO _currentEntitySO;

    public virtual void SetupStats(string key)
    {
        if (!_entitySODict.ContainsKey(key))
        {
            Entity_SO entityClone = GetEntitySOClone(key);
            if(entityClone == null)
            {
                Debug.LogError($"Key-{key}에 해당하는 Entity_SO가 없습니다.");
                return;
            }

            _entitySODict.Add(key, entityClone);
        }
        _currentEntitySO = _entitySODict[key];
    }

    private Entity_SO GetEntitySOClone(string key)
    {
        Entity_SO entitySO = ResourceManager.Instance.Load<Entity_SO>(key);

        return entitySO.Clone() as Entity_SO;
    }

    public void GetDamaged(float damage)
    {

    }

    protected Entity_SO GetEntitySO => _currentEntitySO;

    private void Dead() => OnDead?.Invoke();
}
