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

    [Header("Results")]
    [SerializeField] private GameObject _resultObject;

    [SerializeField] private TMP_Text _atkResultText;
    [SerializeField] private TMP_Text _magazineResultText;
    [SerializeField] private TMP_Text _fireRateResultText;
    [SerializeField] private TMP_Text _rangeResultText;
    [Space(10)]
    [SerializeField] private TMP_Text _atkResultValueText;
    [SerializeField] private TMP_Text _magazineResultValueText;
    [SerializeField] private TMP_Text _fireRateResultValueText;
    [SerializeField] private TMP_Text _rangeResultValueText;

    private Weapon_SO _currentWeapon;
    private string[] _weaponKeys;
    private int _weaponIndex;    

    protected override void Init()
    {
        base.Init();
        _weaponKeys = Enum.GetNames(typeof(Define.WeaponKeys));
        _currentWeapon = ResourceManager.Instance.Load<Weapon_SO>(_weaponKeys[0]);

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
        WeaponManager.Instance.CraftWeapon(_currentWeapon);
        ShowCreateResult();
    }

    private void OnPrevOrNextWeaponBtn(int value)
    {
        int prev = _weaponIndex;
        _weaponIndex = Mathf.Clamp(_weaponIndex + value, 0, (int)Define.WeaponKeys.Count - 1);
        
        if (prev == _weaponIndex)
            return;
        
        _currentWeapon = ResourceManager.Instance.Load<Weapon_SO>(_weaponKeys[_weaponIndex]);
        UpdateWeaponInfo();
    }

    private void UpdateWeaponInfo()
    {
        _weaponImage.sprite = _currentWeapon.ProfileSprite;
        _weaponDescValueText.text = _currentWeapon.DisplayDesc;
        _atkValueText.text = $"{(int)_currentWeapon.Damage}";
        _magazineValueText.text = $"{_currentWeapon.Magazine}";
        _fireRateValueText.text = $"{(int)_currentWeapon.FireRate}";
        _rangeValueText.text = $"{(int)_currentWeapon.FireRange}";
    }

    private void ShowCreateResult()
    {
        _resultObject.SetActive(true);
        //To Do - 실제 랜덤 수치 입히면서 Info Text 갱신
    }

    public override void Close()
    {
        _resultObject.SetActive(false);
        base.Close();
    }
}
