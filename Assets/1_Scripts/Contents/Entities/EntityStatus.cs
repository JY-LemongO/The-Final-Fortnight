using System;
using UnityEngine;

public abstract class EntityStatus
{
    #region Events
    public event Action<float, float> OnHPValueChanged;
    public event Action OnDead;
    #endregion

    public string CodeName { get; protected set; }
    public float MaxHp { get; protected set; }
    public float Hp { get; protected set; }
    public float HPBarOffset { get; protected set; }

    public bool IsDead
    {
        get => _isDead;
        protected set
        {
            _isDead = value;
            OnDead?.Invoke();
            DebugUtility.Log($"[EntityStatus] IsDead = {_isDead}");
        }
    }
    private bool _isDead;

    public virtual void SetupStatus(Entity_SO so)
    {
        CodeName = so.CodeName;
        MaxHp = so.Hp;
        Hp = so.Hp;
        HPBarOffset = so.HPBarOffset;
    }

    public virtual void GetDamaged(float damage)
    {
        Hp = Mathf.Max(Hp - damage, 0);
        OnHPValueChanged?.Invoke(Hp, MaxHp);

        if (Hp <= 0)
            IsDead = true;
    }

    public virtual void ResetStatus()
    {
        IsDead = false;
        Hp = MaxHp;
    }
}
