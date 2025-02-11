using UnityEngine;

public class ZombieStatus : EntityGenericStatus<Zombie_SO>
{
    public float MoveSpeed { get; private set; }
    public float Range { get; private set; }
    public float Attack { get; private set; }
    public float AttackRate { get; private set; }    

    protected override void ApplyUniqueStats(Zombie_SO so)
    {
        MoveSpeed = so.MoveSpeed;
        Range = so.Range;
        Attack = so.Atk;
        AttackRate = so.AtkRate;
    }
}
