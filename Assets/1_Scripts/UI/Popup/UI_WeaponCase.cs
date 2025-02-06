using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_WeaponCase : UI_Popup
{
    [SerializeField] private Transform _contentsTrs;

    [Header("Buttons")]
    [SerializeField] private Button _closeBtn;

    [Header("Infos")]
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _weaponNameText;
    [SerializeField] private TMP_Text _damageValueText;
    [SerializeField] private TMP_Text _magazineValueText;
    [SerializeField] private TMP_Text _fireRateValueText;
    [SerializeField] private TMP_Text _rangeValueText;

    List<UI_WeaponSlot> _slots = new();

    protected override void Init()
    {
        base.Init();
        BackpackInitialize();
        WeaponManager.Instance.OnWeaponCreated += SetupWeaponUI;
        UI_WeaponSlot.OnWeaponSelected += UpdateWeaponInfo;

        _closeBtn.onClick.AddListener(Close);
    }

    private void OnEnable()
    {
        ResetSelectableWeapon();
    }

    private void ResetSelectableWeapon()
    {
        if (_slots.Count > 0)
            UI_WeaponSlot.SetSelectable(_slots[0]);
    }       

    private void UpdateWeaponInfo(Weapon_SO weapon)
    {
        _icon.sprite = weapon.ProfileSprite;
        _weaponNameText.text = weapon.DisplayName;
        _damageValueText.text = weapon.DamageSO.Value.ToString();
        _magazineValueText.text = weapon.MagazineSO.Value.ToString();
        _fireRateValueText.text = weapon.FireRateSO.Value.ToString();
        _rangeValueText.text = weapon.FireRangeSO.Value.ToString();
    }

    private void BackpackInitialize()
    {
        for (int i = 0; i < Constants.InventorySlotCount; i++)
        {
            _slots.Add(UIManager.Instance.CreateItemUI<UI_WeaponSlot>(_contentsTrs));
        }            

        foreach(var weapon in WeaponManager.Instance.GetAllWeapons())
            SetupWeaponUI(weapon);
    }

    private void SetupWeaponUI(Weapon_SO weapon)
    {
        foreach (UI_WeaponSlot slot in _slots)
        {
            if (!slot.IsSetup)
            {
                slot.Setup(weapon);
                break;
            }
        }
    }
}
