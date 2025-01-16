using UnityEngine;

public class InGamePlayTestScene : MonoBehaviour
{
    [Header("Debugging Setting")]
    [SerializeField] private float spawnDelayTime;
    [SerializeField] private int spawnCount;

    private WaitForSeconds spawnDelay;

    public void OnSpawnSurvivorBtn()
        => SurvivorManager.Instance.SpawnSurvivor(Constants.Key_S_Soldier_01);

    public void OnSpawnZombieBtn()
        => ZombieManager.Instance.SpawnZombie(Constants.Key_Z_Normal_01);
}
