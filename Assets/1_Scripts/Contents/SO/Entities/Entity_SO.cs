using UnityEngine;

public class Entity_SO : BaseSO
{
    [Header("Common Stat")]
    [SerializeField] private Stat_SO _hpSO;
    [SerializeField] private float _hpBarOffset;

    private Stat_SO _statHp;

    public Stat_SO Hp => _statHp;
    public float HPBarOffset => _hpBarOffset;

    public virtual void InitializeStats()
        => _statHp = _hpSO.Clone() as Stat_SO;

    public virtual void ResetStats()
        => Hp.ResetCurrentValue();

    public override object Clone()
    {
        var entityClone = Instantiate(this);
        entityClone.InitializeStats();

        return entityClone;
    }
}
