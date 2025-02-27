using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class BuildingSystem : SingletonBase<BuildingSystem>
{
    #region Events
    public event Action OnBarricateBuilt;
    public event Action OnBarricateDestroyed;
    #endregion

    private List<Barricate> _barricates = new();
    private StructureData _currentStructure;
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

        switch (_currentStructure.structureType)
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

    public void EnterBuildMode(StructureData structure)
    {
        _currentStructure = structure;        

        _buildModeUI = UIManager.Instance.OpenPopupUI<UI_BuildMode>();
        GameObject go = ResourceManager.Instance.Instantiate(Constants.Key_PreviewObject);
        Sprite previewSprite = ResourceManager.Instance.Load<Sprite>(_currentStructure.previewSpriteKey);
        go.GetComponent<PreviewObject>().SetPreview(previewSprite);
        
        // 프리뷰 오브젝트 생성 및 셋업
    }

    public void UpgradeStructure(Barricate barricate)
    {

    }

    public void RepairStructure(Barricate barricate)
    {

    }

    public StructureData GetStructureData(int id)
    {
        if (!DataManager.Instance.StructureData.TryGetValue(id, out StructureData data))
        {
            DebugUtility.LogError($"[BuildingSystem] {id} 에 해당하는 StructureData가 없습니다.");
            return null;
        }
        return data;
    }

    private void BuildBarricate(GameObject go)
    {
        StructureData barricateData = GetStructureData(_currentStructure.id);
        Barricate barricate = go.GetComponent<Barricate>();
        barricate.SetupByData(barricateData);
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
