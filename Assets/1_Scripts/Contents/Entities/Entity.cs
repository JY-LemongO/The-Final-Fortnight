using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{    
    public event Action<Entity> OnDead;

    public Entity_SO CurrentEntitySO { get; private set; }
    public Vector3 HPBarPosition { get; private set; }
    public bool IsDead => CurrentEntitySO.Hp.CurrentValue <= 0f;

    protected Dictionary<string, Entity_SO> _entitySODict = new();
    
    protected Animator _animator;
    protected SpriteRenderer _renderer;
    protected EntityDamageEffect _damageEffect;
    [SerializeField]protected UI_HPBar _hpBar;    

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {        
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        _damageEffect = GetComponent<EntityDamageEffect>();        
    }

    protected Entity_SO GetEntitySO => CurrentEntitySO;

    public virtual void SetupEntity<T>(string key) where T : Entity_SO
    {
        if (!_entitySODict.ContainsKey(key))
        {
            T entityClone = GetEntitySOClone<T>(key);
            if (entityClone == null)
            {
                Debug.LogError($"Key-{key}에 해당하는 Entity_SO가 없습니다.");
                return;
            }            
            _entitySODict.Add(key, entityClone);
        }
        CurrentEntitySO = _entitySODict[key] as T;
        HPBarPosition = Vector3.up * CurrentEntitySO.HPBarOffset;

        SetSpriteSortingOrder();
        SetHPBarUI();
    }

    private T GetEntitySOClone<T>(string key) where T : Entity_SO
    {
        T entitySO = ResourceManager.Instance.Load<T>(key);

        return entitySO.Clone() as T;
    }

    private void SetHPBarUI()
    {
        _hpBar = UIManager.Instance.CreateWorldUI<UI_HPBar>(Constants.Key_HPBar);
        _hpBar.SetEntity(this);
    }

    private void SetSpriteSortingOrder()
    {
        int sortingOrderOffsetByPositionY = Mathf.FloorToInt(transform.position.y * 100f);
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

    private void Dead()
    {
        _hpBar.ReturnToPool();
        OnDead?.Invoke(this);
        OnDead = null;
        Dispose();
    }

    protected abstract void Dispose();
}
