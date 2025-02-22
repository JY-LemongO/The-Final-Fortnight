using System;
using UnityEngine;

[RequireComponent(typeof(EntityDamageEffect))]
public abstract class Entity : MonoBehaviour
{
    [field: SerializeField] public Define.EntityType EntityType { get; protected set; }    

    public EntityStatus Status => _status;
    protected EntityStatus _status;
    protected EntityDamageEffect _damageEffect;
    protected SpriteRenderer _renderer;
    protected UI_HPBar _hpBar;
    
    private bool _isInit;

    public virtual void Setup(Entity_SO so)
    {
        if (!_isInit)
            Init();
        _status.SetupStatus(so);
        SetHPBarUI();

        if (this is IAnimatedObject animatedEntity && so.AnimatorController != null)
            animatedEntity.SetAnimatorController(so.AnimatorController);
    }

    public virtual void GetDamaged(float damage)
    {
        if (_status.IsDead)
            return;

        _status.GetDamaged(damage);
        _damageEffect.EffectDamaged();
        SetDamageTextUI(damage);
    }

    public virtual void SetHPBarUI()
    {
        _hpBar = UIManager.Instance.CreateWorldUI<UI_HPBar>();
        _hpBar.SetEntity(this);
        _hpBar.SetHPBarWidth(_status.HPBarWidth);
    }        

    public virtual void SetDamageTextUI(float damage)
    {
        GameObject damageText = ResourceManager.Instance.Instantiate(Constants.Key_DamageText);
        damageText.transform.position = transform.position + Vector3.up * Status.HPBarOffset;
        damageText.GetComponent<DamageText>().SetDamageText(damage);
    }

    protected virtual void Init()
    {
        _isInit = true;
        _status = CreateStatusInstance();

        ComponentsSetting();        
        SetSpriteSortingOrder();
        _status.OnDead += () =>
        {
            if (EntityType == Define.EntityType.MainBarricate)
                GameManager.Instance.GameOver();
        };
    }

    protected virtual void ComponentsSetting()
    {
        _damageEffect = GetComponent<EntityDamageEffect>();
        _damageEffect.Setup(this);
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
