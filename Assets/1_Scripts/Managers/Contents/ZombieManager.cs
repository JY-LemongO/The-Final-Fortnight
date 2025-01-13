using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : SingletonBase<ZombieManager>
{
    private float _spawnPositionX, _minPositionY, _maxPositionY;

    private const string SPAWN_MARKER = "ZombieSpawnPoint";
    private const string POSITION_MARKER_MIN = "Min_PositionY";
    private const string POSITION_MARKER_MAX = "Max_PositionY";
    private const string KEY_ZOMBIE_PREFAB = "Zombie.prefab";

    public ZombieManager() => _isDontDestroy = false;

    protected override void Setup()
    {
        _spawnPositionX = GameObject.Find(SPAWN_MARKER).transform.position.x;
        _minPositionY = GameObject.Find(POSITION_MARKER_MIN).transform.position.y;
        _maxPositionY = GameObject.Find(POSITION_MARKER_MAX).transform.position.y;
    }

    public void Spawn(string key)
    {
        GameObject go = ResourceManager.Instance.Instantiate(KEY_ZOMBIE_PREFAB);
        go.transform.position = new Vector2(_spawnPositionX, Random.Range(_minPositionY, _maxPositionY));

        Zombie zombie = go.GetComponent<Zombie>();
        zombie.SetupStats(key);
    }

    public void Dispawn()
    {

    }

    public override void Dispose()
    {
        
    }
}
