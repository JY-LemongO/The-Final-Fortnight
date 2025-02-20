using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameScene : UI_Scene
{
    [SerializeField] private GameObject _hudObject;    

    [Header("Buttons")]
    [SerializeField] private Button _survivorsBtn;
    [SerializeField] private Button _craftingBtn;
    [SerializeField] private Button _weaponBtn;
    [SerializeField] private Button _inventoryBtn;
    [SerializeField] private Button _radioBtn;

    [Header("In-Game Infos")]
    [SerializeField] private TMP_Text _dayText;
    [SerializeField] private TMP_Text _moneyText;
    [SerializeField] private TMP_Text _batteryText;
    [SerializeField] private TMP_Text _batterRequireValueText;
    
    [Header("Blinder")]
    [SerializeField] private GameObject _blinder;

    private Animator _blinderAnim;

    protected override void Init()
    {
        base.Init();

        InitializeButtonListeners();
        SubscribeEvents();

        _batterRequireValueText.text = $"{GachaManager.Instance.ExecuteRequirement}";

        _blinderAnim = GetComponent<Animator>();
        _blinder.SetActive(true);
        HandleFadeIn();
        
    }

    private void SubscribeEvents()
    {
        GameManager.Instance.OnRestartGame += () =>
        {
            _hudObject.SetActive(false);
            _blinder.SetActive(true);
            _blinderAnim.SetTrigger("FadeOut");
        };
        GameManager.Instance.OnDayChanged += (day) => _dayText.text = $"{day}";
        GameManager.Instance.OnBatteryChanged += (battery) => _batteryText.text = $"{battery}";
        GameManager.Instance.OnMoneyChanged += (money) => _moneyText.text = $"{money}";
        GameManager.Instance.OnStartGame += () => _hudObject.SetActive(true);

        GachaManager.Instance.OnExecuteGacha += (require) => _batterRequireValueText.text = $"{require}";
    }
    
    private void InitializeButtonListeners()
    {
        _survivorsBtn.onClick.AddListener(OnSurvivorsBtn);
        _craftingBtn.onClick.AddListener(OnCraftingBtn);
        _weaponBtn.onClick.AddListener(OnWeaponBtn);
        _inventoryBtn.onClick.AddListener(OnWeaponCaseBtn);
        _radioBtn.onClick.AddListener(OnRadioBtn);
    }

    private void OnSurvivorsBtn()
    {
        // Test Code
        UIManager.Instance.OpenPopupUI<UI_SurvivorSelect>();

        // To Do - 현재 가지고있는 생존자 리스트 보여주기
    }

    private void OnCraftingBtn()
    {
        // To Do - 건축 목록 보여주기
        UIManager.Instance.OpenPopupUI<UI_BuildStructure>();
    }

    private void OnWeaponBtn()
    {
        UIManager.Instance.OpenPopupUI<UI_Weapon>();
    }

    private void OnWeaponCaseBtn()
    {
        UIManager.Instance.OpenPopupUI<UI_WeaponCase>();
    }

    private void OnRadioBtn()
    {
        GachaManager.Instance.Execute();
    }

    private void HandleFadeIn()
    {
        _blinderAnim.SetTrigger("FadeIn");        
    }

    private void HandleSelectSurvivor()
    {
        _blinder.SetActive(false);
        UIManager.Instance.OpenPopupUI<UI_SurvivorSelect>();
    }

    protected override void Dispose()
    {

    }
}
