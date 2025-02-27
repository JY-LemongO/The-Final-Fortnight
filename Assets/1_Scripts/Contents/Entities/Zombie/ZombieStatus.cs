using Data;

public class ZombieStatus : EntityGenericStatusByData<ZombieData>
{
    public int Level { get; private set; }
    public float MoveSpeed { get; private set; }
    public float Range { get; private set; }
    public float Attack { get; private set; }
    public float AttackRate { get; private set; }

    protected override void ApplyUniqueStats(ZombieData data)
    {
        Level = data.level;
        MoveSpeed = data.moveSpeed;
        Range = data.range;
        Attack = data.attack;
        AttackRate = data.attackRate;
    }
}
