using System.Collections;
using UnityEngine;

public class InGamePlayTestScene : MonoBehaviour
{
    [SerializeField] private string key_Animators;
    [SerializeField] private string key_ZombieStats;
    [SerializeField] private string key_ZombieEntities1;
    [SerializeField] private string key_ZombieEntities2;
    [SerializeField] private string key_ZombiePrefab;

    [Header("Debugging Setting")]
    [SerializeField] private float spawnDelayTime;
    [SerializeField] private int spawnCount;

    private WaitForSeconds spawnDelay;

    private void Start()
    {
        spawnDelay = new WaitForSeconds(spawnDelayTime);
        InitLoad();
        StartCoroutine(Co_SpawnZombieInfinite());
    }

    private void InitLoad()
    {
        ResourceManager.Instance.LoadAsync<GameObject>(key_ZombiePrefab);
        ResourceManager.Instance.LoadAsync<Zombie_SO>(key_ZombieEntities1);
        ResourceManager.Instance.LoadAsync<Zombie_SO>(key_ZombieEntities2);
    }

    private IEnumerator Co_SpawnZombieInfinite()
    {
        yield return new WaitForSeconds(5f);

        int count = 0;
        while (count < spawnCount)
        {
            if (count % 2 == 0)
                ZombieManager.Instance.Spawn(key_ZombieEntities1);
            else
                ZombieManager.Instance.Spawn(key_ZombieEntities2);
            count++;

            yield return spawnDelay;
        }
    }

    private void OnDestroy()
    {
        ResourceManager.Instance.Dispose();
    }
}
