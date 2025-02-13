using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_WeaponSlot : UI_Item
{
    #region Events
    public static event Action<WeaponStatus> OnWeaponSelected;
    #endregion

    [SerializeField] private Image _icon;
    [SerializeField] private Image _equipmentImage;
    [SerializeField] private Image _highLight;
    [SerializeField] private TMP_Text _amountText;    
    
    public static UI_WeaponSlot SelectedSlot { get; private set; }
    public static void SetSelectable(UI_WeaponSlot slot)
    {
        SelectedSlot = slot;
        slot.SetHighLight(true);
        OnWeaponSelected?.Invoke(slot.Weapon);
    }
    
    public WeaponStatus Weapon { get; private set; }
    public bool IsSetup { get; private set; } = false;

    protected override void Init()
    {
        base.Init();        
        EmptySlot();

#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += state =>
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                SelectedSlot = null;
                OnWeaponSelected = null;
            }                
        };
#endif
    }

    public void Setup(WeaponStatus weapon)
    {
        IsSetup = true;
        Weapon = weapon;
        _icon.sprite = weapon.ProfileSprite;
        _icon.gameObject.SetActive(true);
        _equipmentImage.gameObject.SetActive(weapon.IsEquiped);
    }

    public void EmptySlot()
    {
        IsSetup = false;
        Weapon = null;
        _icon.gameObject.SetActive(false);
        _highLight.gameObject.SetActive(false);
        _equipmentImage.gameObject.SetActive(false);
        _amountText.text = string.Empty;
        _amountText.gameObject.SetActive(false);
    }

    public void SetHighLight(bool isOn)
        => _highLight.gameObject.SetActive(isOn);

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (Weapon == null || SelectedSlot == this)
            return;

        SetHighLight(true);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (Weapon == null || SelectedSlot == this)
            return;

        bool isInRect = RectTransformUtility.RectangleContainsScreenPoint(_rect, eventData.position);
        if (isInRect)
        {
            if (SelectedSlot != null)
                SelectedSlot.SetHighLight(false);
            SetSelectable(this);
            // To Do - 현재 아이템 정보 띄우기
        }
        else
            SetHighLight(false);
    }
}
