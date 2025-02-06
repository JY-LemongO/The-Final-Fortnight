using System;
using UnityEngine;

[RequireComponent(typeof(EntityDamageEffect))]
public abstract class Entity : MonoBehaviour
{
    #region Renderer
    protected Animator _anim;
    protected SpriteRenderer _renderer;
    #endregion

    #region Events
    public event Action OnDead;
    #endregion

    public Entity_SO CurrentEntitySO { get; protected set; }
    public Define.EntityType EntityType { get; protected set; }
    public Vector3 HPBarPosition { get; protected set; }
    public bool IsDead => CurrentEntitySO.Hp.CurrentValue <= 0f;
    
    protected EntityDamageEffect _damageEffect;    
    protected UI_HPBar _hpBar;    

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        _anim = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        _damageEffect = GetComponent<EntityDamageEffect>();        

        _anim = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.sortingOrder = Util.GetSortingOreder(Define.SpriteType.Weapon);
        
        _damageEffect.Init();
        AnimationHashInitialize();
    }

    public virtual void SetupEntity<T>(T entityClone) where T : Entity_SO
    {
        string key = entityClone.CodeName;

        CurrentEntitySO = entityClone;
        HPBarPosition = Vector3.up * CurrentEntitySO.HPBarOffset;

        _damageEffect.Setup(this);
        SetSpriteSortingOrder();
        SetHPBarUI();
    }

    private void SetHPBarUI()
    {
        _hpBar = UIManager.Instance.CreateWorldUI<UI_HPBar>();
        _hpBar.SetEntity(this);
    }

    private void SetSpriteSortingOrder()
    {
        int sortingOrderOffsetByPositionY = Mathf.FloorToInt(transform.position.y * 100f) * -1;
        _renderer.sortingOrder = Util.GetSortingOreder(Define.SpriteType.Entity) + sortingOrderOffsetByPositionY;
    }

    private void SetupDamageText(float damage)
    {
        GameObject damageText = ResourceManager.Instance.Instantiate(Constants.Key_DamageText);
        damageText.transform.position = transform.position + Vector3.up * CurrentEntitySO.HPBarOffset;
        damageText.GetComponent<DamageText>().SetDamageText(damage);
    }

    public void GetDamaged(Entity attacker, float damage)
    {
        if (IsDead)
            return;
        
        CurrentEntitySO.Hp.Consume(damage);
        _damageEffect.EffectDamaged();
        SetupDamageText(damage);

        if (CurrentEntitySO.Hp.CurrentValue <= 0f)
            Dead();
    }

    protected virtual void Dead()
    {
        _hpBar.Close();
        OnDead?.Invoke();
        OnDead = null;        
    }    

    protected virtual void AnimationHashInitialize() { }

    public virtual void ResetEntity()
    {        
        _hpBar.Close();
        CurrentEntitySO = null;
    }

    public abstract void Dispose();    
}
