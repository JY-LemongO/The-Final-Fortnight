using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : SingletonBase<ZombieManager>
{
    public List<Zombie> ZombiesList { get; private set; } = new();

    private float _spawnPositionX, _minPositionY, _maxPositionY;

    private const string ZOMBIE_SPAWN_MARKER = "ZombieSpawnPoint";
    private const string POSITION_MARKER_MIN = "Min_PositionY";
    private const string POSITION_MARKER_MAX = "Max_PositionY";
    private const string KEY_ZOMBIE_PREFAB = "Zombie.prefab";

    public ZombieManager() => _isDontDestroy = false;

    public void SpawnZombie(string key)
    {
        GameObject go = ResourceManager.Instance.Instantiate(KEY_ZOMBIE_PREFAB);
        go.transform.position = new Vector2(_spawnPositionX, Random.Range(_minPositionY, _maxPositionY));

        Zombie zombie = go.GetComponent<Zombie>();
        zombie.SetupEntity<Zombie_SO>(key);
        ZombiesList.Add(zombie);
    }

    public void Dispawn(Zombie zombie)
    {
        ZombiesList.Remove(zombie);
        PoolManager.Instance.Return(zombie.gameObject);
    }

    protected override void InitChild()
    {
        _spawnPositionX = GameObject.Find(ZOMBIE_SPAWN_MARKER).transform.position.x;
        _minPositionY = GameObject.Find(POSITION_MARKER_MIN).transform.position.y;
        _maxPositionY = GameObject.Find(POSITION_MARKER_MAX).transform.position.y;
    }

    public override void Dispose()
    {
        ZombiesList.Clear();
        base.Dispose();
    }
}
