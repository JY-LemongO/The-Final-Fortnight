using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_FindSurvivor : UI_Popup
{
    [Header("Survivor Infos")]
    [SerializeField] private Image _survivorProfileImage;
    [SerializeField] private TMP_Text _survivorNameText;
    [SerializeField] private TMP_Text _survivorDescText;    

    [Header("Weapon Infos")]
    [SerializeField] private Image _weaponProfileImage;
    [SerializeField] private TMP_Text _weaponNameText;

    [Header("Status")]
    // 생존자 고유 능력치 추가

    [SerializeField] private TMP_Text _damageValueText;
    [SerializeField] private TMP_Text _magazineValueText;
    [SerializeField] private TMP_Text _fireRateValueText;
    [SerializeField] private TMP_Text _fireRangeValueText;    

    [Header("Buttons")]
    [SerializeField] private Button _enlistSurvivorBtn;
    [SerializeField] private Button _rerollBtn;
    [SerializeField] private Button _cancelBtn;

    protected override void ButtonsAddListener()
    {
        _enlistSurvivorBtn.onClick.AddListener(OnEnlistSurvivor);
        _rerollBtn.onClick.AddListener(OnReroll);
        _cancelBtn.onClick.AddListener(Close);
    }

    private void OnEnlistSurvivor()
    {

    }

    private void OnReroll()
    {
        UpdateInfoTexts();
    }

    private void UpdateInfoTexts()
    {

    }
}
