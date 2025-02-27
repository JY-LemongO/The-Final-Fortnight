using Data;
using UnityEngine;

public class Barricate : Entity
{
    public BarricateStatus BarricateStatus { get; private set; }    

    protected override void Init()
    {
        base.Init();
        EntityType = Define.EntityType.Structure;
        BarricateStatus = Status as BarricateStatus;
        BarricateStatus.OnDead += ResetEntity;
    }

    public override void SetupByData(CommonEntityData data)
    {
        base.SetupByData(data);
        _renderer.sprite = BarricateStatus.ObjectSprite;
    }

    public override void GetDamaged(float damage)
    {
        base.GetDamaged(damage);
        DebugUtility.Log($"[Barricate] GetDamaged 오버라이드 함수 내부");
    }

    public override void ResetEntity()
        => PoolManager.Instance.Return(gameObject);

    protected override EntityStatus CreateStatusInstance()
        => new BarricateStatus();    
}
