using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceManager : SingletonBase<ResourceManager>
{
    private Dictionary<string, UnityEngine.Object> _resourcesDict = new();

    public T Load<T>(string key) where T : UnityEngine.Object
    {
        if (_resourcesDict.TryGetValue(key, out UnityEngine.Object obj))
            return obj as T;

        DebugUtility.LogError($"[ResourceManager] Key: {key}에 해당하는 {typeof(T)} 없습니다.");
        return null;
    }

    public GameObject Instantiate(string key, Transform parent = null, bool isPooling = true)
    {
        GameObject prefab = Load<GameObject>(key);

        if (prefab == null)
        {
            Debug.LogError($"로드 실패 :: Key - {key}");
            return null;
        }

        GameObject go = null;

        if (isPooling)
        {
            go = PoolManager.Instance.Get(prefab);
            go.transform.SetParent(parent);
            return go;
        }

        go = Instantiate(prefab, parent);
        go.transform.SetParent(parent);
        go.name = prefab.name;
        return go;
    }

    public void LoadAsync<T>(string key, Action callBack = null) where T : UnityEngine.Object
    {
        var asyncOp = Addressables.LoadAssetAsync<T>(key);
        asyncOp.Completed += (op) =>
        {
            _resourcesDict.Add(key, op.Result);
            callBack?.Invoke();
        };
    }

    public AsyncOperationHandle LoadAllAsyncByLabel<T>(string label, Action<int, int> callBack = null) where T : UnityEngine.Object
    {
        var asyncOp = Addressables.LoadResourceLocationsAsync(label, typeof(T));
        asyncOp.Completed += (op) =>
        {
            int totalCount = op.Result.Count;
            int count = 0;

            foreach (var item in op.Result)
            {
                Debug.Log(item.PrimaryKey);
                LoadAsync<T>(item.PrimaryKey, () =>
                {
                    count++;
                    callBack?.Invoke(count, totalCount);
                });
            }
        };

        return asyncOp;
    }

    protected override void InitChild()
    {

    }

    public override void Dispose()
    {
        _resourcesDict.Clear();
        base.Dispose();
    }
}
