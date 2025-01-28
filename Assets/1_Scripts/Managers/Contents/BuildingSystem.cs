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

    public void PreviewBuildObject()
    {

    }

    public void Build()
    {
        if (_barricates.Count == Constants.Barricate_Buildable)
            return;

        ResourceManager.Instance.Instantiate(Constants.Key_Barricate);
    }

    public void EnterBuildMode()
    {
        // To Do - UI 빌드 모드 팝업 띄우기
        // UI 조건
        // 1. 테두리 강조
        // 2. 중앙 상단 "드래그 하여 설치 지점 선택" 표기
        // 3. 취소 버튼
        // 프리뷰 오브젝트 생성 및 셋업
    }

    public void UpgradeBarricate(Barricate barricate)
    {

    }

    public void RepairBarricate(Barricate barricate)
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
