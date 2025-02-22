using UnityEngine;

public class MainBarricate : Entity
{
    private void Awake()
    {
        Barricate_SO mainBarricateSO = ResourceManager.Instance.Load<Barricate_SO>(Constants.Key_MainBarricate);
        Setup(mainBarricateSO);
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
