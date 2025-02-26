using Data;
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

    private int _currentSurvivorId;    

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
        SurvivorManager.Instance.SpawnSurvivorByData(_currentSurvivorId);
        Close();
    }

    private void OnReroll()
        => GachaManager.Instance.Reroll();

    private void UpdateInfoTexts(SurvivorData survivorData)
    {
        _currentSurvivorId = survivorData.id;
        WeaponData defaultWeapon = WeaponManager.Instance.GetWeaponData(survivorData.defaultWeaponId);

        _survivorProfileImage.sprite = ResourceManager.Instance.Load<Sprite>(survivorData.profileSpriteKey);
        _weaponProfileImage.sprite = ResourceManager.Instance.Load<Sprite>(defaultWeapon.profileSpriteKey);

        _survivorNameText.text = survivorData.displayName;
        _survivorDescText.text = survivorData.displayDesc;
        _weaponNameText.text = defaultWeapon.displayName;

        _damageValueText.text = $"{defaultWeapon.damage}";
        _magazineValueText.text = $"{defaultWeapon.magazine}";
        _fireRateValueText.text = $"{defaultWeapon.fireRate}";
        _fireRangeValueText.text = $"{defaultWeapon.fireRange}";
    }
}
