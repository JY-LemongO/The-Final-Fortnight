using UnityEngine;

public class Entity_SO : BaseSO
{
    [SerializeField] private Stat_SO _hpSO;
    [SerializeField] private Stat_SO _rangeSO;
    [SerializeField] private Stat_SO _atkSO;

    public Stat_SO HPSO => _hpSO;
    public Stat_SO RangeSO => _rangeSO;
    public Stat_SO AtkSO => _atkSO;
}
