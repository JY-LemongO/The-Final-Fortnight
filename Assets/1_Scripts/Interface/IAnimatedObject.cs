using UnityEngine;

public interface IAnimatedObject
{
    Animator Animator { get; }
    void SetAnimatorController(RuntimeAnimatorController controller);    
}
