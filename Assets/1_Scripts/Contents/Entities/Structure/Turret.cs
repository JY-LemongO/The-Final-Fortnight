using UnityEngine;

public class Turret : Entity, IAnimatedObject
{
    public Animator Animator { get; private set; }

    public override void GetDamaged(float damage)
    {
        
    }

    public override void ResetEntity()
    {
        
    }

    public void SetAnimatorController(RuntimeAnimatorController controller)
    {
        
    }

    protected override EntityStatus CreateStatusInstance()
        => new TurretStatus();    
}
