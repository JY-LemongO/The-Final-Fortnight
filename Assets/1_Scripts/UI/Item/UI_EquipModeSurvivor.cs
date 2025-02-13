using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(UIHighlightEffect))]
public class UI_EquipModeSurvivor : UI_Item
{
    #region Event
    public static event Action<Survivor> OnSurvivorSelected;
    #endregion

    [SerializeField] private Image _profileImage;    

    public static UI_EquipModeSurvivor SelectedSurvivorSlot { get; private set; }
    public Survivor Survivor {  get; private set; }
    public bool IsSetup { get; private set; }

    private UIHighlightEffect _highlightEffect;

    protected override void Init()
    {
        base.Init();
        _highlightEffect = GetComponent<UIHighlightEffect>();
        EmptySlot();
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
        _highlightEffect.SetHighlight(false);
        _profileImage.gameObject.SetActive(false);
    }

    public void SetSelectedSurvivor(UI_EquipModeSurvivor slot)
        => SelectedSurvivorSlot = slot;

    public void SetHighLight(bool isHightLight)
        => _highlightEffect.SetHighlight(isHightLight);

    public override void OnPointerDown(PointerEventData eventData)
        => _highlightEffect.SetHighlight(true);

    public override void OnPointerUp(PointerEventData eventData)
    {
        // To Do - 현재 선택된 서바이버의 장착중인 무기와 비교하기
        if (Survivor == null || SelectedSurvivorSlot == this)
            return;

        bool isInRect = RectTransformUtility.RectangleContainsScreenPoint(_rect, eventData.position);
        if (isInRect)
        {
            if (SelectedSurvivorSlot != null)
                SelectedSurvivorSlot.SetHighLight(false);
            SetSelectedSurvivor(this);
            // To Do - 현재 아이템 정보 띄우기
        }
        else
            SetHighLight(false);
    }
}
