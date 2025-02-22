using UnityEngine;

public class StructureStatus : EntityGenericStatus<Structure_SO>
{
    public Define.StructureType StructureType { get; private set; }
    public Sprite ObjectSprite { get; private set; }

    protected override void ApplyUniqueStats(Structure_SO so)
    {
        StructureType = so.StructureType;
        ObjectSprite = so.ObjectSprite;
    }
}
