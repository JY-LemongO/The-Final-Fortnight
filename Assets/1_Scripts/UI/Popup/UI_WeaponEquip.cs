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

    private List<UI_EquipModeSurvivor> _survivorList = new();

    protected override void Init()
    {
        base.Init();        
        UIInitialize();

        SurvivorManager.Instance.OnSurvivorListChanged += UpdateSurvivorList;
        UI_EquipModeSurvivor.OnSurvivorSelected += UpdateWeaponStatus;
    }

    protected override void ButtonsAddListener()
    {
        _closeBtn.onClick.AddListener(Close);
    }

    private void OnEnable()
    {
        ResetCompare();
    }

    private void UIInitialize()
    {
        for(int i = 0; i < Constants.SurvivorSlotCount; i++)
            _survivorList.Add(UIManager.Instance.CreateItemUI<UI_EquipModeSurvivor>(_survivorItemUITrs));

        foreach (var survivor in SurvivorManager.Instance.GetSurvivorsList())
            UpdateSurvivorList(survivor);
    }

    private void UpdateSurvivorList(Survivor survivor)
    {
        foreach(var slot in _survivorList)
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

        _eWeaponProfileImage.sprite = equipedWeapon.ProfileSprite;
        _eWeaponNameText.text = equipedWeapon.DisplayName;
        _eDamageValueText.text = $"{equipedWeapon.Damage}";
        _eMagazineValueText.text = $"{equipedWeapon.MaxMagazine}";
        _eFireRateValueText.text = $"{equipedWeapon.FireRate}";
        _eFireRangeValueText.text = $"{equipedWeapon.FireRange}";

        _sWeaponProfileImage.sprite = selectedWeapon.ProfileSprite;
        _sWeaponNameText.text = selectedWeapon.DisplayName;        
        _sDamageValueText.text = $"{selectedWeapon.Damage}";
        _sMagazineValueText.text = $"{selectedWeapon.MaxMagazine}";
        _sFireRateValueText.text = $"{selectedWeapon.FireRate}";
        _sFireRangeValueText.text = $"{selectedWeapon.FireRange}";
    }

    private void ResetCompare()
    {
        UpdateWeaponStatus(_survivorList[0].Survivor);
    }
}
