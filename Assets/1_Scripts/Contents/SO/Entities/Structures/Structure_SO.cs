using UnityEngine;

public class Structure_SO : Entity_SO
{
    [SerializeField] private Define.StructureType _structureType;
    [SerializeField] private Sprite _objectSprite;
    [SerializeField] private Sprite _previewSprite;

    [SerializeField] private int _cost;

    public Define.StructureType StructureType => _structureType;
    public Sprite ObjectSprite => _objectSprite;
    public Sprite PreviewSprite => _previewSprite;

    public int Cost => _cost;
}
