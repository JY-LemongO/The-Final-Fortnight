using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survivor : Entity
{
    #region Renderer
    [Header("AnimationData")]
    [SerializeField] private string _walkParamName;
    [SerializeField] private string _attackParamName;
    [SerializeField] private string _dieParamName;

    protected int _walkParamHash;
    protected int _attackParamHash;
    protected int _dieParamHash;
    #endregion

    [Header("Weapon")]
    [field:SerializeField] 
    public Weapon Weapon { get; private set; }
    public Zombie Target { get; private set; }

    private Survivor_SO _currentSurvivorSO;
    private Coroutine _targetSearchCoroutine;

    protected override void Init()
    {
        base.Init();
        EntityType = Define.EntityType.Survivor;
    }

    public override void SetupEntity<T>(string key)
    {
        base.SetupEntity<T>(key);
        _currentSurvivorSO = CurrentEntitySO as Survivor_SO;
        _anim.runtimeAnimatorController = _currentSurvivorSO.AnimController;
        
        SetWeapon(_currentSurvivorSO.DefaultWeapon);
        SetBulletUI();
        SearchTarget();        
    }

    public void SearchTarget()
    {
        if (Target != null)
            return;

        if (_targetSearchCoroutine != null)
            StopCoroutine(_targetSearchCoroutine);
        _targetSearchCoroutine = StartCoroutine(Co_SearchTarget());
    }

    public void AttackTarget(float damage)
    {
        if (Target == null)
            return;

        Debug.Log("Attack Target"); 
        Target.GetDamaged(this, damage);
        CheckTargetIsDead();
    }

    public bool CheckTargetIsDead()
    {        
        if (Target!= null && Target.IsDead)
        {
            Target = null;
            SearchTarget();
            return true;
        }
        return false;
    }

    /// <summary>
    /// 사용 할 Weapon 셋팅. 매개변수로 클론화 하지 않은 Weapon_SO 전달.
    /// </summary>    
    public void SetWeapon(Weapon_SO weaponSO)
    {
        Weapon_SO weaponSOClone = weaponSO.Clone() as Weapon_SO;        
        Weapon.SetWeapon(this, weaponSOClone);
        Weapon.transform.localPosition = weaponSOClone.WeaponPosition;
    }

    private void SetBulletUI()
    {
        UI_Bullet ui = UIManager.Instance.CreateWorldUI<UI_Bullet>();
        ui.SetSurvivor(this);
        ui.transform.position = transform.position + Vector3.up * 1.2f;
    }

    private IEnumerator Co_SearchTarget()
    {
        float serachingDelay = Constants.SearchingDelay;
        float fireRange = Weapon.WeaponData.FireRange.Value;
        List<Zombie> zombies = ZombieManager.Instance.ZombiesList;
        while (Target == null)
        {
            if (zombies.Count != 0)
            {
                float closestDistance = float.MaxValue;
                foreach (Zombie zombie in zombies)
                {
                    if (zombie.IsDead)
                        continue;

                    float distance = Vector2.Distance(transform.position, zombie.transform.position);
                    if (distance > fireRange)
                        continue;

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        Target = zombie;
                    }
                }
            }
            yield return Util.GetCachedWaitForSeconds(serachingDelay);
        }
    }

    protected override void Dead()
    {
        _anim.SetTrigger(_dieParamHash);
        base.Dead();
    }

    protected override void AnimationHashInitialize()
    {
        _walkParamHash = Animator.StringToHash(_walkParamName);
        _attackParamHash = Animator.StringToHash(_attackParamName);
        _dieParamHash = Animator.StringToHash(_dieParamName);
    }

    public override void ResetEntity()
    {
        base.ResetEntity();
        Dispose();
    }

    public override void Dispose()
    {
        StopAllCoroutines();
        _currentSurvivorSO = null;
        Target = null;        
        _targetSearchCoroutine = null;
    }
}
