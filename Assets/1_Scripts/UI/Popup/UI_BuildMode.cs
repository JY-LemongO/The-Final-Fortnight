using UnityEngine;
using UnityEngine.UI;

public class UI_BuildMode : UI_Popup
{
    [SerializeField] private Button _cancelBtn;

    protected override void Init()
    {
        base.Init();

        _cancelBtn.onClick.AddListener(OnCancelBtn);
    }

    private void OnCancelBtn()
    {
        Close();
        UIManager.Instance.OpenPopupUI<UI_Crafting>();
    }
}
