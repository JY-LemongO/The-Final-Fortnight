using UnityEngine;

public class Entity_SO : BaseSO
{
    [Header("Common Stat")]
    [SerializeField] private float _hp;
    [SerializeField] private float _hpBarOffset;
    [SerializeField] private float _hpBarWidth;
    [SerializeField] private RuntimeAnimatorController _animController;

    public float Hp => _hp;
    public float HPBarOffset => _hpBarOffset;
    public float HPBarWidth => _hpBarWidth;
    public RuntimeAnimatorController AnimatorController => _animController;
}
