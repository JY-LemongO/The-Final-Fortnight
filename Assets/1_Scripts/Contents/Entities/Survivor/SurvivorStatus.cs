using Data;
using UnityEngine;

public class SurvivorStatus : EntityGenericStatusByData<SurvivorData>
{
    public Sprite ProfileSprite { get; private set; }

    protected override void ApplyUniqueStats(SurvivorData data)
    {
        ProfileSprite = ResourceManager.Instance.Load<Sprite>(data.profileSpriteKey);
    }
}
