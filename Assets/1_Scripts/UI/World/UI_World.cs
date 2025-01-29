using UnityEngine;

public class UI_World : UIBase
{
    protected override void Init()
    {
        UIType = Define.UIType.World;
        GameManager.Instance.OnRestartGame += Dispose;
    }

    protected override void Dispose() 
    {
        PoolManager.Instance.Return(gameObject);
    }    

    protected virtual void OnDestroy()
    {
        GameManager.Instance.OnRestartGame -= Dispose;
    }
}
