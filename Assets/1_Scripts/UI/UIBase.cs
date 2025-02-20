using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    public Define.UIType UIType { get; protected set; }

    private void Awake()
    {
        Init();
    }

    protected virtual void Init() { }    

    public virtual void Close()
        => UIManager.Instance.CloseUI(this);

    protected abstract void Dispose();
    private void OnDisable() => Dispose();
}
