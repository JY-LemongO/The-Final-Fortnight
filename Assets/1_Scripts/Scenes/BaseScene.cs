using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.SceneType CurrentScene { get; protected set; } = Define.SceneType.Unknown;

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        EventSystem eventSystem = FindFirstObjectByType<EventSystem>();
        if(eventSystem == null)
            ResourceManager.Instance.Instantiate("EventSystem").name = "EventSystem";
    }

    public abstract void Dispose();
}
