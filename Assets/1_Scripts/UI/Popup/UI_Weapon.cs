using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Weapon : UI_Popup
{
    [Header("Book Left")]
    [SerializeField] private Button _createWeaponBtn;
    [SerializeField] private Button _nextWeaponBtn;
    [SerializeField] private Button _prevWeaponBtn;

    [SerializeField] private Image _weaponImage;
    [SerializeField] private TMP_Text _weaponDescValueText;
    [SerializeField] private TMP_Text _weaponOwnedValueText;
    [SerializeField] private TMP_Text _weaponCostValueText;    

    [Header("Book Right")]
    [SerializeField] private Button _closeBtn;

    [SerializeField] private TMP_Text _atkValueText;
    [SerializeField] private TMP_Text _magazineValueText;
    [SerializeField] private TMP_Text _fireRateValueText;
    [SerializeField] private TMP_Text _rangeValueText;

    private Weapon_SO _currentWeapon;
    private int _weaponIndex;

    protected override void Init()
    {
        base.Init();
        _currentWeapon = ResourceManager.Instance.Load<Weapon_SO>(Define.WeaponKeys.WEAPON_PISTOL_01.ToString());

        ButtonsAddListener();
        UpdateWeaponInfo();
    }

    private void ButtonsAddListener()
    {
        _closeBtn.onClick.AddListener(Close);
        _createWeaponBtn.onClick.AddListener(OnCreateWeaponBtn);
        _nextWeaponBtn.onClick.AddListener(() => OnPrevOrNextWeaponBtn(1));
        _prevWeaponBtn.onClick.AddListener(() => OnPrevOrNextWeaponBtn(-1));
    }

    private void OnCreateWeaponBtn()
    {
        //WeaponManager.Instance.CraftWeapon();
    }

    private void OnPrevOrNextWeaponBtn(int value)
    {
        _weaponIndex += value;
        string key = Enum.GetNames(typeof(Define.WeaponKeys))[_weaponIndex];

        _currentWeapon = ResourceManager.Instance.Load<Weapon_SO>(key);
        UpdateWeaponInfo();
    }

    private void UpdateWeaponInfo()
    {
        _weaponDescValueText.text = _currentWeapon.DisplayDesc;
        _atkValueText.text = $"{(int)_currentWeapon.Damage.Value}";
        _magazineValueText.text = $"{(int)_currentWeapon.Magazine.Value}";
        _fireRateValueText.text = $"{(int)_currentWeapon.FireRate.Value}";
        _rangeValueText.text = $"{(int)_currentWeapon.FireRange.Value}";
    }
}
