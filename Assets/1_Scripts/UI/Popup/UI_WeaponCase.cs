using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_WeaponCase : UI_Popup
{
    [SerializeField] private Transform _contentsTrs;

    [Header("Buttons")]
    [SerializeField] private Button _closeBtn;
    [SerializeField] private Button _equipBtn;
    [SerializeField] private Button _upgradeBtn;

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
        ButtonsAddListener();

        WeaponManager.Instance.OnWeaponCreated += SetupWeaponUI;
        UI_WeaponSlot.OnWeaponSelected += UpdateWeaponInfo;
    }

    private void ButtonsAddListener()
    {
        _closeBtn.onClick.AddListener(Close);
        _equipBtn.onClick.AddListener(OnEquipWeapon);
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

    private void UpdateWeaponInfo(WeaponStatus weapon)
    {
        _icon.sprite = weapon.ProfileSprite;
        _weaponNameText.text = weapon.DisplayName;
        _damageValueText.text = weapon.Damage.ToString();
        _magazineValueText.text = weapon.MaxMagazine.ToString();
        _fireRateValueText.text = weapon.FireRate.ToString();
        _rangeValueText.text = weapon.FireRange.ToString();
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

    private void SetupWeaponUI(WeaponStatus weapon)
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

    private void OnEquipWeapon()
    {

    }
}
