using UnityEngine;

[CreateAssetMenu(menuName = "Entitity/New Zombie", fileName = "ZOMBIE_")]
public class Zombie_SO : Entity_SO
{
    [SerializeField] private Stat_SO _moveSpeedSO;

    [Header("Visual")]
    [SerializeField] private RuntimeAnimatorController _animController;

    public Stat_SO MoveSpeedSO => _moveSpeedSO;
    public RuntimeAnimatorController AnimController => _animController;
}
