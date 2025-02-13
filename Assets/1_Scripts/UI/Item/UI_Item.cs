using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Item : UIBase, IPointerDownHandler, IPointerUpHandler
{
    protected RectTransform _rect;

    protected override void Init()
    {
        UIType = Define.UIType.Item;
        _rect = GetComponent<RectTransform>();
    }    

    public virtual void OnPointerDown(PointerEventData eventData) { }

    public virtual void OnPointerUp(PointerEventData eventData) { }

    protected override void Dispose()
    {
        
    }    
}
