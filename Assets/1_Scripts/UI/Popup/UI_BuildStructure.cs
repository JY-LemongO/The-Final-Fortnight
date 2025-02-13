using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BuildStructure : UI_Popup
{
    [Header("ViewPorts")]
    [SerializeField] private GameObject _barricateViewPort;
    [SerializeField] private GameObject _turretViewPort;

    [Header("CraftUI Buttons")]
    [SerializeField] private Button _barricateBtn;
    [SerializeField] private Button _turretBtn;
    [SerializeField] private Button _closeBtn;

    [Header("Barricate")]
    [SerializeField] private Button _buildBarricateBtn;

    [SerializeField] private Image _barricateImage;
    [SerializeField] private TMP_Text _barricateHPText;
    [SerializeField] private TMP_Text _barricateBuildCostText;

    [Header("Turret")]
    [SerializeField] private Image _turretImage;

    protected override void Init()
    {
        base.Init();

        _barricateViewPort.SetActive(true);        
        _turretViewPort.SetActive(false);
    }

    protected override void ButtonsAddListener()
    {
        _barricateBtn.onClick.AddListener(OnBarricateBtn);

        _turretBtn.onClick.AddListener(OnTurretBtn);
        _closeBtn.onClick.AddListener(Close);

        _buildBarricateBtn.onClick.AddListener(OnBarricateBuildBtn);
    }

    #region Barricate
    private void OnBarricateBtn()
    {
        if (_barricateViewPort.activeSelf)
            return;

        ChangeViewPort(_barricateViewPort, _turretViewPort);
        UpdateBarricateInfo();
    }

    private void UpdateBarricateInfo()
    {
        Barricate_SO barricateInfo = BuildingSystem.Instance.GetBarricateInfo();
        _barricateImage.sprite = barricateInfo.ObjectSprite;
        _barricateHPText.text = barricateInfo.Hp.ToString();
        _barricateBuildCostText.text = barricateInfo.Cost.ToString();
    }

    private void OnBarricateBuildBtn()
    {
        BuildingSystem.Instance.EnterBuildMode(GetBarricateToStructureSO());
        Close();
    }
    #endregion

    #region Turret
    private void OnTurretBtn()
    {
        if (_turretViewPort.activeSelf)
            return;

        ChangeViewPort(_turretViewPort, _barricateViewPort);

    }
    #endregion   

    private Structure_SO GetBarricateToStructureSO()
    {
        int barricateUpgradeTier = GameManager.Instance.CurrentBarricateUpgrade;
        string key = Enum.GetNames(typeof(Define.BarricateTier))[barricateUpgradeTier];

        return GetStructureSO(key);
    }

    private Structure_SO GetStructureSO(string key)
        => ResourceManager.Instance.Load<Structure_SO>(key);

    private void ChangeViewPort(GameObject enableViewPort, GameObject disableViewPort)
    {
        enableViewPort.SetActive(true);
        disableViewPort.SetActive(false);        
    }
}
