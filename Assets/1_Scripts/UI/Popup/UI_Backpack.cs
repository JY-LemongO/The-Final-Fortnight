using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Backpack : UI_Popup
{
    [SerializeField] private Button _closeBtn;

    [SerializeField] private Transform _contentsTrs;
    List<UI_InventorySlot> _slots = new();

    protected override void Init()
    {
        base.Init();
        BackpackInitialize();
        WeaponManager.Instance.OnWeaponCreated += SetupWeaponUI;

        _closeBtn.onClick.AddListener(Close);
    }

    private void BackpackInitialize()
    {
        for (int i = 0; i < Constants.InventorySlotCount; i++)
        {
            _slots.Add(UIManager.Instance.CreateItemUI<UI_InventorySlot>(_contentsTrs));
        }
            

        foreach(var weapon in WeaponManager.Instance.GetAllWeapons())
            SetupWeaponUI(weapon);
    }

    private void SetupWeaponUI(Weapon_SO weapon)
    {
        foreach (UI_InventorySlot slot in _slots)
        {
            if (!slot.IsSetup)
            {
                slot.Setup(weapon);
                break;
            }
        }
    }
}
