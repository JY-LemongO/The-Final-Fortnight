using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ResourceManager : SingletonBase<ResourceManager>
{
    private Dictionary<string, UnityEngine.Object> _resourcesDict = new();

    public T Load<T>(string key) where T : UnityEngine.Object
    {
        if (_resourcesDict.TryGetValue(key, out UnityEngine.Object obj))
            return obj as T;
        return null;
    }

    public GameObject Instantiate(string key, Transform parent = null, bool isPooling = true)
    {
        GameObject prefab = Load<GameObject>(key);

        if(prefab == null)
        {
            Debug.LogError($"로드 실패 :: Key - {key}");
            return null;
        }

        if (isPooling)
            return PoolManager.Instance.Get(prefab);

        GameObject go = Instantiate(prefab, parent);
        go.name = prefab.name;
        return go;
    }

    public void LoadAsync<T>(string key, Action<T> callBack = null) where T : UnityEngine.Object
    {
        var asyncOp = Addressables.LoadAssetAsync<T>(key);
        asyncOp.Completed += (op) =>
        {
            _resourcesDict.Add(key, op.Result);
            callBack?.Invoke(op.Result);
        };
    }

    public override void Dispose()
    {
        _resourcesDict.Clear();
    }
}
