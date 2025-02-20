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

    [Header("Battery")]
    [SerializeField] private TMP_Text _batteryValueText;

    private string _currentSurvivorKey;    

    protected override void Init()
    {
        base.Init();
        GachaManager.Instance.OnGachaResult += UpdateInfoTexts;
        _batteryValueText.text = $"-{Constants.RerollGachaInitCost}";
    }

    protected override void ButtonsAddListener()
    {
        _enlistSurvivorBtn.onClick.AddListener(OnEnlistSurvivor);
        _rerollBtn.onClick.AddListener(OnReroll);
        _cancelBtn.onClick.AddListener(Close);
    }
    private void OnEnlistSurvivor()
    {
        SurvivorManager.Instance.SpawnSurvivor(_currentSurvivorKey);
        Close();
    }

    private void OnReroll()
        => GachaManager.Instance.Reroll();

    private void UpdateInfoTexts(Survivor_SO survivorData)
    {
        _currentSurvivorKey = survivorData.CodeName;
        Weapon_SO defaultWeapon = survivorData.DefaultWeapon;

        _survivorProfileImage.sprite = survivorData.ProfileSprite;
        _weaponProfileImage.sprite = defaultWeapon.ProfileSprite;

        _survivorNameText.text = survivorData.DisplayName;
        _survivorDescText.text = survivorData.DisplayDesc;
        _weaponNameText.text = defaultWeapon.DisplayName;

        _damageValueText.text = $"{defaultWeapon.Damage}";
        _magazineValueText.text = $"{defaultWeapon.Magazine}";
        _fireRateValueText.text = $"{defaultWeapon.FireRate}";
        _fireRangeValueText.text = $"{defaultWeapon.FireRange}";
    }
}
