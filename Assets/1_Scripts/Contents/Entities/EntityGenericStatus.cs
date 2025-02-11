

public abstract class EntityGenericStatus<T> : EntityStatus where T : Entity_SO
{
    public override void SetupStatus(Entity_SO so)
    {
        if (so is T typeSO)
        {
            base.SetupStatus(so);
            ApplyUniqueStats(typeSO);
        }
        else
            DebugUtility.LogError($"[EntityGenericStatus] InValid SO Type. Expected{typeof(T)}, but got {so.GetType()}");
    }

    protected abstract void ApplyUniqueStats(T so);
}
