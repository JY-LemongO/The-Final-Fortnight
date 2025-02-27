using Data;
using UnityEngine;

public class MainBarricate : Entity
{
    private void Awake()
    {
        BarricateData mainBarricateData = BuildingSystem.Instance.GetStructureData(Constants.MainBarricateId) as BarricateData;
        SetupByData(mainBarricateData);
    }

    protected override void Init()
    {
        base.Init();
        EntityType = Define.EntityType.MainBarricate;
    }

    public override void ResetEntity()
    {
        
    }

    protected override EntityStatus CreateStatusInstance()
        => new BarricateStatus();
}
