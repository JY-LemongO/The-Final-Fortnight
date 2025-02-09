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
    {
        // To Do - Status 인스턴스 만들기
        return null;
    }
}
