using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_WeaponEquip : UI_Popup
{
    [Header("Survivor List")]
    [SerializeField] private Transform _survivorItemUITrs;

    [Header("Equipped Weapon")]
    [SerializeField] private Image _eWeaponProfileImage;
    [SerializeField] private TMP_Text _eWeaponNameText;
    [SerializeField] private TMP_Text _eDamageValueText;
    [SerializeField] private TMP_Text _eMagazineValueText;
    [SerializeField] private TMP_Text _eFireRateValueText;
    [SerializeField] private TMP_Text _eFireRangeValueText;

    [Header("Selected Weapon")]
    [SerializeField] private Image _sWeaponProfileImage;
    [SerializeField] private TMP_Text _sWeaponNameText;
    [SerializeField] private TMP_Text _sDamageValueText;
    [SerializeField] private TMP_Text _sMagazineValueText;
    [SerializeField] private TMP_Text _sFireRateValueText;
    [SerializeField] private TMP_Text _sFireRangeValueText;

    [Header("Buttons")]
    [SerializeField] private Button _closeBtn;
    [SerializeField] private Button _decideBtn;

    [Header("Color")]
    [SerializeField] private Color _worseColor;
    [SerializeField] private Color _betterColor;

    private List<UI_EquipModeSurvivorSlot> _survivorList = new();

    protected override void Init()
    {
        base.Init();
        UIInitialize();
        
        SurvivorManager.Instance.OnSurvivorListChanged += UpdateSurvivorList;
        UI_EquipModeSurvivorSlot.OnSurvivorSelected += UpdateWeaponStatus;
    }

    protected override void ButtonsAddListener()
    {
        _closeBtn.onClick.AddListener(Close);
        _decideBtn.onClick.AddListener(OnDecide);
    }

    private void OnEnable()
    {
        ResetCompare();
    }

    private void UIInitialize()
    {
        for (int i = 0; i < Constants.SurvivorSlotCount; i++)
            _survivorList.Add(UIManager.Instance.CreateItemUI<UI_EquipModeSurvivorSlot>(_survivorItemUITrs));

        foreach (var survivor in SurvivorManager.Instance.GetSurvivorsList())
            UpdateSurvivorList(survivor);
    }

    private void OnDecide()
    {
        Survivor selectedSurvivor = UI_EquipModeSurvivorSlot.SelectedSurvivorSlot.Survivor;
        WeaponStatus selectedWeapon = UI_WeaponSlot.SelectedSlot.Weapon;
        WeaponManager.Instance.EquipWeapon(selectedSurvivor, selectedWeapon);

        Close();
    }

    private void UpdateSurvivorList(Survivor survivor)
    {
        foreach (var slot in _survivorList)
        {
            if (!slot.IsSetup)
            {
                slot.Setup(survivor);
                break;
            }
        }
    }

    private void UpdateWeaponStatus(Survivor survivor)
    {
        WeaponStatus selectedWeapon = UI_WeaponSlot.SelectedSlot.Weapon;
        WeaponStatus equipedWeapon = survivor.Weapon.WeaponStatus;

        _decideBtn.gameObject.SetActive(selectedWeapon != equipedWeapon);

        _eWeaponProfileImage.sprite = equipedWeapon.ProfileSprite;
        _eWeaponNameText.text = equipedWeapon.DisplayName;
        _eDamageValueText.text = $"{equipedWeapon.Damage}";
        _eMagazineValueText.text = $"{equipedWeapon.MaxMagazine}";
        _eFireRateValueText.text = $"{equipedWeapon.FireRate}";
        _eFireRangeValueText.text = $"{equipedWeapon.FireRange}";

        _sWeaponProfileImage.sprite = selectedWeapon.ProfileSprite;
        _sWeaponNameText.text = selectedWeapon.DisplayName;
        _sDamageValueText.text = SetSelectedValueToText(equipedWeapon.Damage, selectedWeapon.Damage);
        _sMagazineValueText.text = SetSelectedValueToText(equipedWeapon.MaxMagazine, selectedWeapon.MaxMagazine);
        _sFireRateValueText.text = SetSelectedValueToText(equipedWeapon.FireRate, selectedWeapon.FireRate);
        _sFireRangeValueText.text = SetSelectedValueToText(equipedWeapon.FireRange, selectedWeapon.FireRange);

        _sDamageValueText.color = SetStatusValueTextColor(equipedWeapon.Damage, selectedWeapon.Damage);
        _sMagazineValueText.color = SetStatusValueTextColor(equipedWeapon.MaxMagazine, selectedWeapon.MaxMagazine);
        _sFireRateValueText.color = SetStatusValueTextColor(selectedWeapon.FireRate, equipedWeapon.FireRate);
        _sFireRangeValueText.color = SetStatusValueTextColor(equipedWeapon.FireRange, selectedWeapon.FireRange);
    }

    private string SetSelectedValueToText(float equippedValue, float selectedValue)
    {
        float diff = selectedValue - equippedValue;
        float diffAbs = Mathf.Abs(diff);
        char sign = diff < 0 ? '-' : '+';

        return $"{selectedValue} ({sign}{diffAbs})";
    }

    private Color SetStatusValueTextColor(float equippedValue, float selectedValue)
    {
        if (Mathf.Approximately(equippedValue, selectedValue))
            return Color.white;

        return equippedValue < selectedValue ? _betterColor : _worseColor;
    }

    private void ResetCompare()
    {
        if(_survivorList.Count == 0)
        {
            DebugUtility.LogError("[UI_WeaponEquip] SurvivorSlot UI 리스트가 비었습니다.");
            return;
        }

        UI_EquipModeSurvivorSlot firstSurvivorSlot = _survivorList[0];        
        firstSurvivorSlot.SetSelectedSurvivor(firstSurvivorSlot);

        foreach (var survivorSlot in _survivorList)
        {
            if (survivorSlot.Survivor == null)
                continue;

            if (survivorSlot.Survivor.Weapon.WeaponStatus == UI_WeaponSlot.SelectedSlot.Weapon)
                survivorSlot.SetEquippedMark(true);
            else
                survivorSlot.SetEquippedMark(false);
        }
    }
}
