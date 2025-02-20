using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InfoPopup : UI_Popup
{
    [Header("Buttons")]
    [SerializeField] private Button _closeBtn;

    [Header("Info")]
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _descText;

    public void Setup(Sprite icon, string name, string desc)
    {
        _iconImage.sprite = icon;
        _nameText.text = name;
        _descText.text = desc;
    }

    protected override void ButtonsAddListener()
    {
        _closeBtn.onClick.AddListener(Close);
    }
}
