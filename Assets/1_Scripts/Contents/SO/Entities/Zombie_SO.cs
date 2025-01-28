using UnityEngine;

[CreateAssetMenu(menuName = "Entity/New Zombie", fileName = "ZOMBIE_")]
public class Zombie_SO : Entity_SO
{
    [Header("Zombie Stats")]
    [SerializeField] private Stat_SO _moveSpeedSO;
    [SerializeField] private Stat_SO _rangeSO;
    [SerializeField] private Stat_SO _atkSO;
    [SerializeField] private Stat_SO _atkRateSO;
    [Header("Visual")]
    [SerializeField] private RuntimeAnimatorController _animController;

    private Stat_SO _statMoveSpeed;
    private Stat_SO _statRange;
    private Stat_SO _statAtk;
    private Stat_SO _statAtkRate;

    public Stat_SO MoveSpeed => _statMoveSpeed;
    public Stat_SO Range => _statRange;
    public Stat_SO Atk => _statAtk;
    public Stat_SO AtkRate => _statAtkRate;
    public RuntimeAnimatorController AnimController => _animController;

    public override void InitializeStats()
    {
        base.InitializeStats();
        _statMoveSpeed = _moveSpeedSO.Clone() as Stat_SO;
        _statRange = _rangeSO.Clone() as Stat_SO;
        _statAtk = _atkSO.Clone() as Stat_SO;
        _statAtkRate = _atkRateSO.Clone() as Stat_SO;
    }

    public override void ResetStats()
    {
        base.ResetStats();
        MoveSpeed.ResetCurrentValue();
        Range.ResetCurrentValue();
        Atk.ResetCurrentValue();
        AtkRate.ResetCurrentValue();
    }

    public override object Clone()
    {
        var zombieClone = Instantiate(this);
        zombieClone.InitializeStats();

        return zombieClone;
    }
}
