using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SurvivorSelect : UI_Popup
{
    [Header("Profile")]
    [SerializeField] private Image _survivorImage;
    [SerializeField] private Image _weaponImage;
    [SerializeField] private TMP_Text _survivornameText;
    [SerializeField] private TMP_Text _weaponNameText;
    [SerializeField] private TMP_Text _descriptionText;

    [Header("Stats")]
    //[SerializeField] private TMP_Text _hpValueText;
    [SerializeField] private TMP_Text _damageValueText;
    [SerializeField] private TMP_Text _magazineValueText;
    [SerializeField] private TMP_Text _fireRateValueText;
    [SerializeField] private TMP_Text _RangeValueText;

    [Header("Buttons")]
    [SerializeField] private Button _prevBtn;
    [SerializeField] private Button _nextBtn;
    [SerializeField] private Button _selectBtn;

    private int _currentIndex = 0;
    private string _currentSurvivorKey = string.Empty;

    private void Awake()
    {
        base.Init();

        _currentSurvivorKey = Constants.Key_S_Soldier_01;
        Survivor_SO initSurvivor = ResourceManager.Instance.Load<Survivor_SO>(_currentSurvivorKey);        

        OnUpdateProfile(initSurvivor);
        OnUpdateStatsValue(initSurvivor);
    }

    protected override void ButtonsAddListener()
    {
        _prevBtn.onClick.AddListener(() => OnPrevOrNextBtn(-1));
        _nextBtn.onClick.AddListener(() => OnPrevOrNextBtn(1));
        _selectBtn.onClick.AddListener(OnSelectBtn);
    }

    #region Profile
    private void OnUpdateProfile(Survivor_SO survivor)
    {
        _survivorImage.sprite = survivor.ProfileSprite;
        _weaponImage.sprite = survivor.DefaultWeapon.ProfileSprite;

        _survivornameText.text = survivor.DisplayName;
        _weaponNameText.text = survivor.DefaultWeapon.DisplayName;
        _descriptionText.text = survivor.DisplayDesc;
    }
    #endregion

    #region Stats
    private void OnUpdateStatsValue(Survivor_SO survivor)
    {
        //_hpValueText.text = survivor.Hp.ToString();
        _damageValueText.text = survivor.DefaultWeapon.Damage.ToString();
        _magazineValueText.text = survivor.DefaultWeapon.Magazine.ToString();
        _fireRateValueText.text = survivor.DefaultWeapon.FireRate.ToString();
        _RangeValueText.text = survivor.DefaultWeapon.FireRange.ToString();
    }
    #endregion

    #region Event_Buttons
    private void OnPrevOrNextBtn(int buttonValue)
    {
        int prevIndex = _currentIndex;
        _currentIndex = Mathf.Clamp(_currentIndex + buttonValue, 0, SurvivorManager.Instance.SelectableSurvivorList.Count - 1);
        if (_currentIndex == prevIndex)
            return;

        Survivor_SO survivor = SurvivorManager.Instance.GetSelectableSurvivor(_currentIndex);
        OnUpdateProfile(survivor);
        OnUpdateStatsValue(survivor);
    }

    private void OnSelectBtn()
    {
        string survivorKey = Enum.GetNames(typeof(Define.SurvivorKeys))[_currentIndex];
        SurvivorManager.Instance.SpawnSurvivor(survivorKey);
        //GameManager.Instance.StartGame();
        Close();
    }

    protected override void Dispose()
    {
        _currentIndex = 0;
        _currentSurvivorKey = string.Empty;
    }
    #endregion
}
