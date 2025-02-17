using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(UIHighlightEffect))]
public class UI_EquipModeSurvivorSlot : UI_Item
{
    #region Event
    public static event Action<Survivor> OnSurvivorSelected;
    #endregion

    [SerializeField] private Image _profileImage;
    [SerializeField] private GameObject _equippedMark;

    public static UI_EquipModeSurvivorSlot SelectedSurvivorSlot
    {
        get => _selectedSurvivorSlot;
        private set
        {
            if (_selectedSurvivorSlot != null)
                _selectedSurvivorSlot.SetHighlight(false);
            _selectedSurvivorSlot = value;

            if (value == null)
                return;

            _selectedSurvivorSlot.SetHighlight(true);
            OnSurvivorSelected?.Invoke(value.Survivor);
        }
    }    
    private static UI_EquipModeSurvivorSlot _selectedSurvivorSlot;

    public Survivor Survivor {  get; private set; }
    public bool IsSetup { get; private set; }

    private UIHighlightEffect _highlightEffect;

    protected override void Init()
    {
        base.Init();
        _highlightEffect = GetComponent<UIHighlightEffect>();
        EmptySlot();

#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += state =>
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                SelectedSurvivorSlot = null;
                OnSurvivorSelected = null;
            }
        };
#endif
    }

    public void Setup(Survivor survivor)
    {
        IsSetup = true;
        Survivor = survivor;

        _profileImage.gameObject.SetActive(true);
        _profileImage.sprite = survivor.SurvivorStatus.ProfileSprite;
    }

    private void EmptySlot()
    {
        IsSetup = false;
        Survivor = null;
        SetHighlight(false);
        SetEquippedMark(false);
        _profileImage.gameObject.SetActive(false);
    }

    public void SetSelectedSurvivor(UI_EquipModeSurvivorSlot slot)
        => SelectedSurvivorSlot = slot;

    public void SetEquippedMark(bool enable)
        => _highlightEffect.SetHighlight(enable, _equippedMark);

    public void SetHighlight(bool enable)
        => _highlightEffect.SetHighlight(enable);

    public override void OnPointerUp(PointerEventData eventData)
    {
        // To Do - 현재 선택된 서바이버의 장착중인 무기와 비교하기
        if (Survivor == null || SelectedSurvivorSlot == this)
            return;

        bool isInRect = RectTransformUtility.RectangleContainsScreenPoint(_rect, eventData.position);
        if (isInRect)
            SetSelectedSurvivor(this);
        else
            SetHighlight(false);
    }
}
