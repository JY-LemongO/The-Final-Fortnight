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

    public bool IsSetup { get; private set; } = false;

    // static이라 이거 에디터 플레이모드 종료될 때 밀어버려야 함.
    private static UI_WeaponSlot _selectedSlot;
    public static void SetSelectable(UI_WeaponSlot slot)
    {
        _selectedSlot = slot;
        slot.SetHighLight(true);
        OnWeaponSelected?.Invoke(slot._weapon);
    }

    private WeaponStatus _weapon;
    private RectTransform _rect;

    protected override void Init()
    {
        base.Init();
        _rect = GetComponent<RectTransform>();
        EmptySlot();

#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += state =>
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                _selectedSlot = null;
                OnWeaponSelected = null;
            }                
        };
#endif
    }

    public void Setup(WeaponStatus weapon)
    {
        IsSetup = true;
        _weapon = weapon;
        _icon.sprite = weapon.ProfileSprite;
        _icon.gameObject.SetActive(true);
        _equipmentImage.gameObject.SetActive(weapon.IsEquiped);
    }

    public void EmptySlot()
    {
        IsSetup = false;
        _weapon = null;
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
        if (_weapon == null || _selectedSlot == this)
            return;

        SetHighLight(true);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (_weapon == null || _selectedSlot == this)
            return;

        bool isInRect = RectTransformUtility.RectangleContainsScreenPoint(_rect, eventData.position);
        if (isInRect)
        {
            if (_selectedSlot != null)
                _selectedSlot.SetHighLight(false);
            SetSelectable(this);
            // To Do - 현재 아이템 정보 띄우기
        }
        else
            SetHighLight(false);
    }
}
