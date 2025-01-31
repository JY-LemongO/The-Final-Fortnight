using UnityEngine;

public class Barricate : Entity
{
    private Barricate_SO _currentBarricateSO;

    protected override void Init()
    {
        base.Init();
        EntityType = Define.EntityType.Structure;
    }

    public override void SetupEntity<T>(string key)
    {
        base.SetupEntity<T>(key);
        _currentBarricateSO = CurrentEntitySO as Barricate_SO;
        _renderer.sprite = _currentBarricateSO.ObjectSprite;
    }

    public override void Dispose()
    {
        
    }
}
