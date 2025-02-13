using UnityEngine;

public class SurvivorStatus : EntityGenericStatus<Survivor_SO>
{
    // To Do - 나중에 탐색 전용이나 별에 별 능력치 들어가면 될듯
    public Sprite ProfileSprite { get; private set; }

    protected override void ApplyUniqueStats(Survivor_SO so)
    {
        ProfileSprite = so.ProfileSprite;
    }
}
