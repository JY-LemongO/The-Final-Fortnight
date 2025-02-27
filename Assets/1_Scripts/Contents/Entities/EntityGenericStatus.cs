using Data;

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
