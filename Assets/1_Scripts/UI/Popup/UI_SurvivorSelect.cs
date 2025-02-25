using System;
using System.Linq;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SurvivorSelect : UI_Popup
{
    [Header("Profile")]
    [SerializeField] private Image _survivorImage;
    [SerializeField] private Image _weaponImage;
    [SerializeField] private TMP_Text _survivornameText;    
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _weaponNameText;

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
    private int _currentSurvivorId = -1;
    private string _currentSurvivorKey = string.Empty;    

    private void Awake()
    {
        base.Init();

        //InitializeSurvivorBySO();
        InitializeSurvivorByData();
    }

    protected override void ButtonsAddListener()
    {
        _prevBtn.onClick.AddListener(() => OnPrevOrNextBtn(-1));
        _nextBtn.onClick.AddListener(() => OnPrevOrNextBtn(1));
        _selectBtn.onClick.AddListener(OnSelectBtn);
    }

    private void InitializeSurvivorBySO()
    {
        _currentSurvivorKey = Constants.Key_S_Soldier_01;
        Survivor_SO initSurvivor = ResourceManager.Instance.Load<Survivor_SO>(_currentSurvivorKey);

        OnUpdateProfileBySO(initSurvivor);
        OnUpdateStatsValueBySO(initSurvivor);
    }

    private void InitializeSurvivorByData()
    {
        _currentSurvivorId = DataManager.Instance.SurvivorData.Keys.First();
        SurvivorData initSurvivor = DataManager.Instance.SurvivorData[_currentSurvivorId];

        OnUpdateProfileByData(initSurvivor);
        OnUpdateStatsValueByData(initSurvivor);
    }

    #region Profile
    private void OnUpdateProfileBySO(Survivor_SO survivorSO)
    {
        _survivorImage.sprite = survivorSO.ProfileSprite;
        _weaponImage.sprite = survivorSO.DefaultWeapon.ProfileSprite;

        _survivornameText.text = survivorSO.DisplayName;        
        _descriptionText.text = survivorSO.DisplayDesc;
        _weaponNameText.text = survivorSO.DefaultWeapon.DisplayName;
    }

    private void OnUpdateProfileByData(SurvivorData survivorData)
    {
        ResourceManager resourceManager = ResourceManager.Instance;        
        WeaponData weaponData = WeaponManager.Instance.GetWeaponData(survivorData.defaultWeaponId);

        _survivorImage.sprite = resourceManager.Load<Sprite>(survivorData.profileSpriteKey);
        _weaponImage.sprite = resourceManager.Load<Sprite>(weaponData.profileSpriteKey);

        _survivornameText.text = survivorData.displayName;        
        _descriptionText.text = survivorData.displayDesc;
        _weaponNameText.text = weaponData.displayName;
    }
    #endregion

    #region Stats
    private void OnUpdateStatsValueBySO(Survivor_SO survivor)
    {
        //_hpValueText.text = survivor.Hp.ToString();
        _damageValueText.text = survivor.DefaultWeapon.Damage.ToString();
        _magazineValueText.text = survivor.DefaultWeapon.Magazine.ToString();
        _fireRateValueText.text = survivor.DefaultWeapon.FireRate.ToString();
        _RangeValueText.text = survivor.DefaultWeapon.FireRange.ToString();
    }

    private void OnUpdateStatsValueByData(SurvivorData survivor)
    {
        WeaponData weaponData = WeaponManager.Instance.GetWeaponData(survivor.defaultWeaponId);

        //_hpValueText.text = survivor.Hp.ToString();
        _damageValueText.text = weaponData.damage.ToString();
        _magazineValueText.text = weaponData.magazine.ToString();
        _fireRateValueText.text = weaponData.fireRate.ToString();
        _RangeValueText.text = weaponData.fireRange.ToString();
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
        OnUpdateProfileBySO(survivor);
        OnUpdateStatsValueBySO(survivor);
    }

    private void OnSelectBtn()
    {
        //string survivorKey = Enum.GetNames(typeof(Define.SurvivorKeys))[_currentIndex];
        //SurvivorManager.Instance.SpawnSurvivorBySO(survivorKey);

        SurvivorManager.Instance.SpawnSurvivorByData(_currentSurvivorId);

        Close();
    }

    protected override void Dispose()
    {
        _currentIndex = 0;
        _currentSurvivorKey = string.Empty;
    }
    #endregion
}
