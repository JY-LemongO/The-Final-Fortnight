using UnityEngine;

public class Barricate : Entity
{
    private Barricate_SO _currentBarricateSO => CurrentEntitySO as Barricate_SO;

    protected override void Init()
    {
        base.Init();
        EntityType = Define.EntityType.Structure;
    }

    public override void SetupEntity<T>(T entityClone)
    {
        if(entityClone is not Barricate_SO)
        {
            Debug.LogError($"SetupEntity: 잘못된 Entity_SO 타입입니다. {typeof(T)}");
            return;
        }

        base.SetupEntity(entityClone);        
        _renderer.sprite = _currentBarricateSO.ObjectSprite;
    }

    public override void Dispose()
    {
        
    }
}
