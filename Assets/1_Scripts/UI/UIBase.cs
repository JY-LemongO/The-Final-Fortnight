using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    private void Awake()
    {
        Init();
    }

    protected virtual void Init() { }

    public virtual void Setup() { }

    public virtual void Close()
        => UIManager.Instance.CloseUI(this);

    protected abstract void Dispose();
    private void OnDisable() => Dispose();
}
