using UnityEngine;

public class Barricate : Entity
{
    protected override void Init()
    {
        base.Init();
        EntityType = Define.EntityType.Structure;
    }

    public override void ResetEntity()
    {
        
    }

    protected override EntityStatus CreateStatusInstance()
    {
        // To Do - Status 만들기
        return null;
    }
}
