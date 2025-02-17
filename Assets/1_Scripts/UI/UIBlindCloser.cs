using UnityEngine;
using UnityEngine.EventSystems;

public class UIBlindCloser : MonoBehaviour, IPointerClickHandler
{
    private UI_Popup _popup;

    public void SetPopup(UI_Popup popup)
        => _popup = popup;

    public void OnPointerClick(PointerEventData eventData)
        => _popup.Close();
}
