using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInteractableImage : MonoBehaviour, IPointerClickHandler
{    
    [SerializeField] private string _displayName;
    [TextArea][SerializeField] private string _displayDesc;
    private Sprite _icon;

    private void Awake()
    {
        InitInfo();
    }

    private void InitInfo()
        => _icon = GetComponent<Image>().sprite;

    public void OnPointerClick(PointerEventData eventData)
    {
        DebugUtility.Log("클릭 됨");
        UIManager.Instance.OpenPopupUI<UI_InfoPopup>().Setup(_icon, _displayName, _displayDesc);
    }
}
