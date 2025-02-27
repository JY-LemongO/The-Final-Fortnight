using System;
using System.Collections.Generic;
using System.Linq;
using Data;
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

    private List<int> _structureIdList = new();
    private StructureData _currentStructure;

    protected override void Init()
    {
        base.Init();

        _barricateViewPort.SetActive(true);        
        _turretViewPort.SetActive(false);
        _structureIdList = DataManager.Instance.StructureData.Keys.ToList();
        _structureIdList.Sort();
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
        BarricateData barricateData = _currentStructure as BarricateData;        

        _barricateImage.sprite = ResourceManager.Instance.Load<Sprite>(barricateData.objectSpriteKey);
        _barricateHPText.text = barricateData.hp.ToString();
        _barricateBuildCostText.text = barricateData.buildCost.ToString();
    }

    private void OnBarricateBuildBtn()
    {
        BuildingSystem.Instance.EnterBuildMode(GetBarricateToStructureData());
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

    private StructureData GetBarricateToStructureData()
    {
        int barricateUpgradeTier = GameManager.Instance.CurrentBarricateUpgrade;
        int id = _structureIdList[barricateUpgradeTier - 1];

        return GetStructureData(id);
    }

    private StructureData GetStructureData(int id)
        => BuildingSystem.Instance.GetStructureData(id);

    private void ChangeViewPort(GameObject enableViewPort, GameObject disableViewPort)
    {
        enableViewPort.SetActive(true);
        disableViewPort.SetActive(false);        
    }
}
