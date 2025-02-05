using UnityEngine;
using UnityEngine.UI;

public class UI_GameScene : UI_Scene
{
    [Header("Buttons")]
    [SerializeField] private Button _survivorsBtn;
    [SerializeField] private Button _craftingBtn;
    [SerializeField] private Button _weaponBtn;
    [SerializeField] private Button _inventoryBtn;
    [SerializeField] private Button _radioBtn;

    [Header("Blinder")]
    [SerializeField] private GameObject _blinder;

    private Animator _blinderAnim;

    protected override void Init()
    {
        base.Init();

        _blinderAnim = GetComponent<Animator>();

        _survivorsBtn.onClick.AddListener(OnSurvivorsBtn);
        _craftingBtn.onClick.AddListener(OnCraftingBtn);
        _weaponBtn.onClick.AddListener(OnWeaponBtn);
        _inventoryBtn.onClick.AddListener(OnInventoryBtn);
        _radioBtn.onClick.AddListener(OnRadioBtn);

        HandleFadeIn();
        GameManager.Instance.OnRestartGame += () =>
        {
            _blinder.SetActive(true);
            _blinderAnim.SetTrigger("FadeOut");
        };
    }

    private void OnSurvivorsBtn()
    {
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

    private void OnInventoryBtn()
    {
        UIManager.Instance.OpenPopupUI<UI_WeaponCase>();
    }

    private void OnRadioBtn()
    {
        // 랜덤 생존자 가챠
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
