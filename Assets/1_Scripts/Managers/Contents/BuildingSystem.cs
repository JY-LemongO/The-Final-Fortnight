using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : SingletonBase<BuildingSystem>
{
    #region Events
    public event Action OnBarricateBuilt;
    public event Action OnBarricateDestroyed;
    #endregion

    private List<Barricate> _barricates = new();
    private Structure_SO _currentStructure;

    private UIBase _buildModeUI;

    public void Build(Vector3 buildPosition)
    {
        if (_barricates.Count == Constants.Barricate_Buildable)
            return;

        if (_buildModeUI != null)
        {
            _buildModeUI.Close();
            _buildModeUI = null;
        }            

        GameObject go = ResourceManager.Instance.Instantiate(Constants.Key_Barricate);
        go.transform.position = buildPosition;

        switch (_currentStructure.StructureType)
        {
            case Define.StructureType.Barricate:
                BuildBarricate(go);
                break;
            case Define.StructureType.Turret:
                BuildTurret(go);
                break;
            default:
                Debug.LogError("Type miss.");
                break;
        }
    }

    public void EnterBuildMode(Structure_SO structure)
    {
        _currentStructure = structure;

        _buildModeUI = UIManager.Instance.OpenPopupUI<UI_BuildMode>();
        GameObject go = ResourceManager.Instance.Instantiate(Constants.Key_PreviewObject);
        go.GetComponent<PreviewObject>().SetPreview(_currentStructure.PreviewSprite);
        
        // 프리뷰 오브젝트 생성 및 셋업
    }

    public void UpgradeStructure(Barricate barricate)
    {

    }

    public void RepairStructure(Barricate barricate)
    {

    }

    public Barricate_SO GetBarricateInfo()
    {
        int tier = GameManager.Instance.CurrentBarricateUpgrade;
        string key = Enum.GetNames(typeof(Define.BarricateTier))[tier];
        return ResourceManager.Instance.Load<Barricate_SO>(key).Clone() as Barricate_SO;
    }

    private void BuildBarricate(GameObject go)
    {
        Barricate barricate = go.GetComponent<Barricate>();
        //barricate.SetupEntity<Barricate_SO>(_currentStructure.CodeName);
    }

    private void BuildTurret(GameObject go)
    {

    }

    protected override void InitChild()
    {
        _isDontDestroy = false;
    }

    public override void Dispose()
    {


        base.Dispose();
    }
}
