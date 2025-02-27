using Data;
using UnityEngine;

public class StructureStatus : EntityGenericStatusByData<StructureData>
{
    public Define.StructureType StructureType { get; private set; }
    public Sprite ObjectSprite { get; private set; }

    protected override void ApplyUniqueStats(StructureData so)
    {
        StructureType = so.structureType;
        ObjectSprite = ResourceManager.Instance.Load<Sprite>(so.objectSpriteKey);
    }
}
