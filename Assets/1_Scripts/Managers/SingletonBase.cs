using UnityEditor;
using UnityEngine;

public abstract class SingletonBase<T> : MonoBehaviour where T : SingletonBase<T>
{
    private static T _instance;
    private static bool _isInitialized = false;

    // 자식 클래스에서 설정 및 DontDestroy 설정
    protected bool _isDontDestroy = true;

    public static T Instance { get { if (!_isInitialized) Init(); return _instance; } }

    private static void Init()
    {
        _isInitialized = true;
        GameObject go = GameObject.Find($"@{typeof(T).Name}");
        if (go == null)
            go = new GameObject($"@{typeof(T).Name}", typeof(T));

        _instance = go.GetComponent<T>();
        if (_instance._isDontDestroy)
            DontDestroyOnLoad(go);

        _instance.InitChild();

        EditorApplication.playModeStateChanged += state =>
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
                _instance.Dispose();
        };
    }

    protected abstract void InitChild();

    public virtual void Dispose()
    {
        _isInitialized = false;
    }
}
