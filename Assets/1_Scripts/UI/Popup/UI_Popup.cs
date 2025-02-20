using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UI_Popup : UIBase
{
    [Header("없어도 됨")]
    [SerializeField] private UIBlindCloser _blindCloser;

    protected override void Init()
    {
        UIType = Define.UIType.Popup;
        ButtonsAddListener();

        if (_blindCloser != null)
            _blindCloser.SetPopup(this);
    }

    protected abstract void ButtonsAddListener();

    protected override void Dispose() { }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }
}
