using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Item : UIBase, IPointerDownHandler, IPointerUpHandler
{
    protected override void Init()
    {
        UIType = Define.UIType.Item;
    }    

    public virtual void OnPointerDown(PointerEventData eventData) { }

    public virtual void OnPointerUp(PointerEventData eventData) { }

    protected override void Dispose()
    {
        
    }    
}
