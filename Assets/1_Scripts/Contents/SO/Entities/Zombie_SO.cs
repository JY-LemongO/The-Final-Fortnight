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

    public Stat_SO MoveSpeed => _moveSpeedSO;
    public Stat_SO Range => _rangeSO;
    public Stat_SO Atk => _atkSO;
    public Stat_SO AtkRate => _atkRateSO;
    public RuntimeAnimatorController AnimController => _animController;
}
