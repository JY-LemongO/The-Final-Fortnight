using Data;

public abstract class EntityGenericStatus<T> : EntityStatus where T : Entity_SO
{
    public override void SetupStatusBySO(Entity_SO so)
    {
        if (so is T typeSO)
        {
            base.SetupStatusBySO(so);
            ApplyUniqueStats(typeSO);
        }
        else
            DebugUtility.LogError($"[EntityGenericStatus] InValid SO Type. Expected{typeof(T)}, but got {so.GetType()}");
    }

    protected abstract void ApplyUniqueStats(T so);
}

public abstract class EntityGenericStatusByData<T> : EntityStatus where T : CommonEntityData
{
    public override void SetupStatusByData(CommonEntityData data)
    {
        if (data is T typeData)
        {
            base.SetupStatusByData(data);
            ApplyUniqueStats(typeData);
        }
        else
            DebugUtility.LogError($"[EntityGenericStatus] InValid SO Type. Expected{typeof(T)}, but got {data.GetType()}");
    }

    protected abstract void ApplyUniqueStats(T data);
}
