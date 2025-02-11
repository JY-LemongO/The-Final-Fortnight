using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorController : MonoBehaviour
{
    private Survivor _context;
    private Coroutine _targetSearchCoroutine;

    public void Setup(Survivor context)
        => _context = context;

    public void SearchTarget()
    {
        if (_context.Target != null)
            return;

        if (_targetSearchCoroutine != null)
            StopCoroutine(_targetSearchCoroutine);
        _targetSearchCoroutine = StartCoroutine(Co_SearchTarget());
    }

    private IEnumerator Co_SearchTarget()
    {
        float serachingDelay = Constants.SearchingDelay;
        float fireRange = _context.Weapon.WeaponStatus.FireRange;        
        while (_context.Target == null)
        {
            List<Zombie> zombies = ZombieManager.Instance.ZombiesList;
            if (zombies.Count != 0)
            {
                float closestDistance = float.MaxValue;
                foreach (Zombie zombie in zombies)
                {
                    if (zombie.Status.IsDead)
                        continue;

                    float distance = Vector2.Distance(transform.position, zombie.transform.position);
                    if (distance > fireRange)
                        continue;

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        _context.SetTarget(zombie);
                    }
                }
            }

            yield return Util.GetCachedWaitForSeconds(serachingDelay);
        }
    }
}
