using UnityEngine;

public class Entity_SO : BaseSO
{
    [Header("Common Stat")]
    [SerializeField] private Stat_SO _hpSO;
    [SerializeField] private float _hpBarOffset;
    [SerializeField] private RuntimeAnimatorController _animController;

    public Stat_SO Hp => _hpSO;
    public float HPBarOffset => _hpBarOffset;
    public RuntimeAnimatorController AnimatorController => _animController;
}
