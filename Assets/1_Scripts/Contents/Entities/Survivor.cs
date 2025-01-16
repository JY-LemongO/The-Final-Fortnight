using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survivor : Entity
{
    [SerializeField] private Transform _weaponSlot;

    private Survivor_SO _currentSurvivorSO;
    public Weapon CurrentWeapon { get; private set; }
    [field: SerializeField] public Zombie Target { get; private set; }

    private Coroutine _targetSearchCoroutine;

    public override void SetupEntity<T>(string key)
    {
        base.SetupEntity<T>(key);
        _currentSurvivorSO = CurrentEntitySO as Survivor_SO;
        _animator.runtimeAnimatorController = _currentSurvivorSO.AnimController;

        GameObject weaponClone = ResourceManager.Instance.Instantiate("Weapon.prefab", _weaponSlot);
        weaponClone.transform.localPosition = Vector3.zero;

        Weapon_SO weaponSOClone = _currentSurvivorSO.DefaultWeapon.Clone() as Weapon_SO;
        CurrentWeapon = weaponClone.GetComponent<Weapon>();
        CurrentWeapon.InitializeWeapon(this, weaponSOClone);

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
        Target.GetDamaged(this, damage);
        CheckTargetIsDead();
    }

    public bool CheckTargetIsDead()
    {
        if (Target.IsDead)
        {
            Target = null;
            SearchTarget();
            return true;
        }
        return false;
    }

    private IEnumerator Co_SearchTarget()
    {
        float serachingDelay = Constants.SearchingDelay;
        float fireRange = CurrentWeapon.WeaponData.FireRange.Value;
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

    protected override void Dispose()
    {
        _currentSurvivorSO = null;
        Target = null;
        CurrentWeapon = null;
        _targetSearchCoroutine = null;
    }
}
