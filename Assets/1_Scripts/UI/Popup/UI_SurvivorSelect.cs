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
    [SerializeField] private TMP_Text _hpValueText;
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

        _prevBtn.onClick.AddListener(() => OnPrevOrNextBtn(-1));
        _nextBtn.onClick.AddListener(() => OnPrevOrNextBtn(1));
        _selectBtn.onClick.AddListener(OnSelectBtn);
    }

    #region Profile
    private void OnUpdateProfile(Survivor_SO survivor)
    {
        Survivor_SO statInitializedSurvivor = survivor.Clone() as Survivor_SO;
        Weapon_SO statInitializedWeapon = survivor.DefaultWeapon.Clone() as Weapon_SO;

        _survivorImage.sprite = statInitializedSurvivor.ProfileSprite;
        _weaponImage.sprite = statInitializedWeapon.ProfileSprite;

        _survivornameText.text = statInitializedSurvivor.DisplayName;
        _weaponNameText.text = statInitializedWeapon.DisplayName;
        _descriptionText.text = statInitializedSurvivor.DisplayDesc;
    }
    #endregion

    #region Stats
    private void OnUpdateStatsValue(Survivor_SO survivor)
    {
        Survivor_SO statInitializedSurvivor = survivor.Clone() as Survivor_SO;
        Weapon_SO statInitializedWeapon = survivor.DefaultWeapon.Clone() as Weapon_SO;

        _hpValueText.text = statInitializedSurvivor.Hp.Value.ToString();
        _damageValueText.text = statInitializedWeapon.Damage.Value.ToString();
        _magazineValueText.text = statInitializedWeapon.Magazine.Value.ToString();
        _fireRateValueText.text = statInitializedWeapon.FireRate.Value.ToString();
        _RangeValueText.text = statInitializedWeapon.FireRange.Value.ToString();
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
        string survivorKey = Enum.GetNames(typeof(Define.SurvivorKeys))[_currentIndex] + ".asset";
        SurvivorManager.Instance.SpawnSurvivor(survivorKey);
        GameManager.Instance.StartGame();
        Close();
    }

    protected override void Dispose()
    {
        _currentIndex = 0;
        _currentSurvivorKey = string.Empty;
    }
    #endregion
}
