using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_Crafting : UI_Popup
{
    [Header("ViewPorts")]
    [SerializeField] private GameObject _barricateViewPort;
    [SerializeField] private GameObject _weaponViewPort;
    [SerializeField] private GameObject _turretViewPort;

    [Header("Buttons")]
    [SerializeField] private Button _barricateBtn;
    [SerializeField] private Button _weaponBtn;
    [SerializeField] private Button _turretBtn;
    [SerializeField] private Button _closeBtn;

    [SerializeField] private Button _buildBarricateBtn;

    [Header("Images")]
    [SerializeField] private Image _barricateImage;
    [SerializeField] private Image _weaponImage;
    [SerializeField] private Image _turretImage;

    protected override void Init()
    {
        base.Init();

        _barricateViewPort.SetActive(true);
        _weaponViewPort.SetActive(false);
        _turretViewPort.SetActive(false);

        ButtonsAddListener();
    }

    private void ButtonsAddListener()
    {
        _barricateBtn.onClick.AddListener(OnBarricateBtn);
        _weaponBtn.onClick.AddListener(OnWeaponBtn);
        _turretBtn.onClick.AddListener(OnTurretBtn);
        _closeBtn.onClick.AddListener(Close);

        _buildBarricateBtn.onClick.AddListener(OnBarricateBuildBtn);
    }

    #region Barricate
    private void OnBarricateBtn()
    {
        if (_barricateViewPort.activeSelf)
            return;

        ChangeViewPort(_barricateViewPort, _weaponViewPort, _turretViewPort);

    }

    private void OnBarricateBuildBtn()
    {        
        BuildingSystem.Instance.EnterBuildMode(GetBarricateToStructureSO());
        Close();
    }
    #endregion

    #region Weapon
    private void OnWeaponBtn()
    {
        if (_weaponViewPort.activeSelf)
            return;

        ChangeViewPort(_weaponViewPort, _barricateViewPort, _turretViewPort);
    }

    private void OnWeaponAssembleBtn()
    {

    }
    #endregion

    #region Turret
    private void OnTurretBtn()
    {
        if (_turretViewPort.activeSelf)
            return;

        ChangeViewPort(_turretViewPort, _barricateViewPort, _weaponViewPort);

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

    private void ChangeViewPort(GameObject enableViewPort, GameObject disableViewPort1, GameObject disableViewPort2)
    {
        enableViewPort.SetActive(true);
        disableViewPort1.SetActive(false);
        disableViewPort2.SetActive(false);
    }
}
