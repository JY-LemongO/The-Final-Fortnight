using UnityEngine;

public abstract class UI_Popup : UIBase
{
    protected override void Init()
    {
        UIType = Define.UIType.Popup;
        ButtonsAddListener();
    }

    protected abstract void ButtonsAddListener();

    protected override void Dispose() { }    
}
