using System.Collections.Generic;
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

    private List<int> _survivorsList = new();
    private SurvivorData _currentSurvivor;
    private int _currentIndex = 0;    

    private void Awake()
    {
        base.Init();

        _survivorsList = DataManager.Instance.SelectableSurvivorsData.Keys.ToList();
        InitializeSurvivorByData();
    }

    protected override void ButtonsAddListener()
    {
        _prevBtn.onClick.AddListener(() => OnPrevOrNextBtn(-1));
        _nextBtn.onClick.AddListener(() => OnPrevOrNextBtn(1));
        _selectBtn.onClick.AddListener(OnSelectBtn);
    }

    private void InitializeSurvivorByData()
    {
        _currentSurvivor = SurvivorManager.Instance.GetSurvivorData(_survivorsList[0]);

        OnUpdateProfileByData(_currentSurvivor);
        OnUpdateStatsValueByData(_currentSurvivor);
    }

    #region Profile
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
        int total = _survivorsList.Count;
        _currentIndex = Mathf.Clamp(_currentIndex + buttonValue, 0, total - 1);
        if (_currentIndex == prevIndex)
            return;

        _currentSurvivor = SurvivorManager.Instance.GetSurvivorData(_survivorsList[_currentIndex]);
        OnUpdateProfileByData(_currentSurvivor);
        OnUpdateStatsValueByData(_currentSurvivor);
    }

    private void OnSelectBtn()
    {        
        SurvivorManager.Instance.SpawnSurvivorByData(_currentSurvivor.id);
        Close();
    }

    protected override void Dispose()
    {
        _currentIndex = 0;
        _survivorsList.Clear();
    }
    #endregion
}
