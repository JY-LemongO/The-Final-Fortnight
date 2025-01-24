using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    #region renderer
    [Header("AnimationData")]
    [SerializeField] private string _walkParamName;
    [SerializeField] private string _attackParamName;
    [SerializeField] private string _dieParamName;    

    public Animator AnimForTest => _anim;
    protected Animator _anim;
    protected SpriteRenderer _renderer;
    
    protected int _walkParamHash;
    protected int _attackParamHash;
    protected int _dieParamHash;
    #endregion

    public event Action<Entity> OnDead;

    public Entity_SO CurrentEntitySO { get; private set; }
    public Vector3 HPBarPosition { get; private set; }
    public bool IsDead => CurrentEntitySO.Hp.CurrentValue <= 0f;

    protected Dictionary<string, Entity_SO> _entitySODict = new();
    
    protected EntityDamageEffect _damageEffect;
    protected Material _mat;

    private UI_HPBar _hpBar;
    private Coroutine _hitCoroutine;

    private float _hitFlashTime;
    private string _flashAmountKey;

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        _anim = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        _damageEffect = GetComponent<EntityDamageEffect>();
        _mat = _renderer.material;

        _anim = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.sortingOrder = Util.GetSortingOreder(Define.SpriteType.Weapon);

        _hitFlashTime = Constants.HitFlashTime;
        _flashAmountKey = Constants.HitFlashAmountShaderKey;

        AnimationHashInitialize();
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

        if (_hitCoroutine != null)
            StopCoroutine(_hitCoroutine);
        _hitCoroutine = StartCoroutine(Co_HitFlash());

        if (CurrentEntitySO.Hp.CurrentValue <= 0f)
            Dead();
    }

    private void Dead()
    {
        _anim.SetTrigger(_dieParamHash);
        _mat.SetFloat(_flashAmountKey, 0f);
        _hpBar.Close();
        OnDead?.Invoke(this);
        OnDead = null;        
    }

    private IEnumerator Co_HitFlash()
    {
        _mat.SetFloat(_flashAmountKey, 1f);
        yield return Util.GetCachedWaitForSeconds(_hitFlashTime);
        _mat.SetFloat(_flashAmountKey, 0f);
    }

    private void AnimationHashInitialize()
    {
        _walkParamHash = Animator.StringToHash(_walkParamName);
        _attackParamHash = Animator.StringToHash(_attackParamName);
        _dieParamHash = Animator.StringToHash(_dieParamName);        
    }

    protected abstract void Dispose();
}
