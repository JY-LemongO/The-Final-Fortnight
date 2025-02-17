using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorController : MonoBehaviour
{
    private Survivor _context;
    private Coroutine _targetSearchCoroutine;

    private void Awake()
    {
        _context = GetComponent<Survivor>();
    }

    public void SearchTarget(float searchRange)
    {
        if (_targetSearchCoroutine != null)
            StopCoroutine(_targetSearchCoroutine);
        _targetSearchCoroutine = StartCoroutine(Co_SearchTarget(searchRange));
    }

    private IEnumerator Co_SearchTarget(float searchRange)
    {
        float serachingDelay = Constants.SearchingDelay;        
        while (true)
        {
            List<Zombie> zombies = ZombieManager.Instance.ZombiesList;
            if (zombies.Count != 0)
            {
                float closestDistance = float.MaxValue;
                Zombie closestZombie = null;
                foreach (Zombie zombie in zombies)
                {
                    if (zombie.Status.IsDead)
                        continue;

                    float distance = Vector2.Distance(transform.position, zombie.transform.position);
                    if (distance > searchRange)
                        continue;

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestZombie = zombie;                                
                    }
                }

                if(closestZombie != null)
                {
                    _context.SetTarget(closestZombie);
                    break;
                }                
            }
            yield return Util.GetCachedWaitForSeconds(serachingDelay);
        }
        _targetSearchCoroutine = null;
    }
}
