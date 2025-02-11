using System;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    #region Events
    public event Action<float> OnDamaged;
    public event Action OnDead;
    #endregion

    [field: SerializeField] public Define.EntityType EntityType { get; protected set; }
    public bool IsDead
    {
        get => _isDead;
        protected set
        {
            _isDead = value;
            OnDead?.Invoke();
            DebugUtility.Log($"[Entity] IsDead = {_isDead}");
        }
    }

    public EntityStatus Status => _status;
    protected EntityStatus _status;
    protected SpriteRenderer _renderer;

    private bool _isDead;
    private bool _isInit;

    public virtual void Setup(Entity_SO so)
    {
        if (!_isInit)
            Init();
        _status.SetupStatus(so);

        if (this is IAnimatedObject animatedEntity && so.AnimatorController != null)
            animatedEntity.SetAnimatorController(so.AnimatorController);
    }

    public virtual void GetDamaged(float damage)
    {
        if (IsDead)
            return;

        _status.GetDamaged(damage);
        if (_status.Hp <= 0)
            IsDead = true;
    }

    protected virtual void Init()
    {
        _isInit = true;
        _status = CreateStatusInstance();

        ComponenetsSetting();        
        SetSpriteSortingOrder();
    }

    protected virtual void ComponenetsSetting()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.sortingOrder = Util.GetSortingOreder(Define.SpriteType.Weapon);
    }

    private void SetSpriteSortingOrder()
    {
        int sortingOrderOffsetByPositionY = Mathf.FloorToInt(transform.position.y * 100f) * -1;
        _renderer.sortingOrder = Util.GetSortingOreder(Define.SpriteType.Entity) + sortingOrderOffsetByPositionY;
    }

    public abstract void ResetEntity();
    protected abstract EntityStatus CreateStatusInstance();    
}
