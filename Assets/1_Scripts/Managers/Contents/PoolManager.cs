using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

#region Pool
public class Pool
{
    public GameObject originPrefab;

    private IObjectPool<GameObject> _pool;    

    public Pool(GameObject prefab)
    {
        originPrefab = prefab;
        _pool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestroy);
    }

    public void Push(GameObject go)
        => _pool.Release(go);

    public GameObject Pop()
        => _pool.Get();

    #region Pool Args
    private GameObject OnCreate()
    {
        GameObject go = GameObject.Instantiate(originPrefab);
        go.name = originPrefab.name;
        return go;
    }

    private void OnGet(GameObject go)
        => go.SetActive(true);

    private void OnRelease(GameObject go)
        => go.SetActive(false);

    private void OnDestroy(GameObject go)
        => GameObject.Destroy(go);
    #endregion
}
#endregion

public class PoolManager : SingletonBase<PoolManager>
{
    private Dictionary<string, Pool> _poolDict = new();

    public GameObject Get(GameObject prefab)
    {
        if(!_poolDict.ContainsKey(prefab.name))
            CreatePool(prefab);

        return _poolDict[prefab.name].Pop();
    }

    public void Return(GameObject go)
    {
        if (!_poolDict.ContainsKey(go.name))
        {
            Debug.LogError($"Key - {go.name} 가 존재하지 않습니다.");
            return;
        }

        _poolDict[go.name].Push(go);
    }

    private void CreatePool(GameObject prefab)
        => _poolDict.Add(prefab.name, new Pool(prefab));

    public override void Dispose()
    {
        _poolDict.Clear();
    }
}
