using UnityEngine;

public class ZombieStatus : EntityGenericStatus<Zombie_SO>
{
    public float MoveSpeed { get; private set; }
    public float Range { get; private set; }
    public float Attack { get; private set; }
    public float AttackRate { get; private set; }    

    protected override void ApplyUniqueStats(Zombie_SO so)
    {
        // To Do - so의 StatSO 로직 갈아 엎을 예정
        MoveSpeed = so.MoveSpeed.Value;
        Range = so.Range.Value;
        Attack = so.Atk.Value;
        AttackRate = so.AtkRate.Value;
    }
}
