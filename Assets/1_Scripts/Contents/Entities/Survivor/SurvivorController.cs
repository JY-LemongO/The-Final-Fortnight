using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorController : MonoBehaviour
{
    private Survivor _context;
    private Coroutine _targetSearchCoroutine;

    public void SearchTarget()
    {
        if (_context.Target != null)
            return;

        if (_targetSearchCoroutine != null)
            StopCoroutine(_targetSearchCoroutine);
        _targetSearchCoroutine = StartCoroutine(Co_SearchTarget());
    }

    public void AttackTarget(float damage)
    {
        if (_context.Target == null)
            return;

        Debug.Log("Attack Target");
        _context.Target.GetDamaged(damage);
        CheckTargetIsDead();
    }

    public bool CheckTargetIsDead()
    {
        if (_context.Target != null && _context.Target.IsDead)
        {            
            SearchTarget();
            return true;
        }
        return false;
    }

    private IEnumerator Co_SearchTarget()
    {
        float serachingDelay = Constants.SearchingDelay;
        float fireRange = _context.Weapon.WeaponStatus.FireRange;
        List<Zombie> zombies = ZombieManager.Instance.ZombiesList;
        while (_context.Target == null)
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
                        // To Do - Target 원래 여기서 정했었는데 없앴음.
                    }
                }
            }
            yield return Util.GetCachedWaitForSeconds(serachingDelay);
        }
    }
}
