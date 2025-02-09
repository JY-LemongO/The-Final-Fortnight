using Unity.VisualScripting;
using UnityEngine;

public abstract class EntityStatus
{
    public string CodeName { get; protected set; }
    public float MaxHp { get; protected set; }
    public float Hp { get; protected set; }
    public float HPBarOffset { get; protected set; }
    
    public virtual void SetupStatus(Entity_SO so)
    {
        CodeName = so.CodeName;
        MaxHp = so.Hp.Value;
        Hp = so.Hp.Value;
        HPBarOffset = so.HPBarOffset;
    }

    public virtual void GetDamaged(float damage)
    {
        Hp = Mathf.Max(Hp - damage, 0);
    }
}
