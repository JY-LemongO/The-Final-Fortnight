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

    public void SpawnZombie(string zombieSOKey)
    {
        Zombie_SO zombieSO = ResourceManager.Instance.Load<Zombie_SO>(zombieSOKey);
        Zombie zombie = NewZombie();
        zombie.Setup(zombieSO);

        ZombiesList.Add(zombie);
    }

    public void Dispawn(Zombie zombie)
    {
        ZombiesList.Remove(zombie);
        PoolManager.Instance.Return(zombie.gameObject);
    }    

    private Zombie NewZombie()
    {
        GameObject go = ResourceManager.Instance.Instantiate(KEY_ZOMBIE_PREFAB);
        go.transform.position = new Vector2(_spawnPositionX, Random.Range(_minPositionY, _maxPositionY));

        return go.GetComponent<Zombie>();
    }

    private void OnRestartGame()
    {
        foreach(var zombie in ZombiesList)
        {
            zombie.ResetEntity();
            PoolManager.Instance.Return(zombie.gameObject);
        }            

        ZombiesList.Clear();
    }

    protected override void InitChild()
    {
        GameManager.Instance.OnRestartGame += OnRestartGame;

        _spawnPositionX = GameObject.Find(ZOMBIE_SPAWN_MARKER).transform.position.x;
        _minPositionY = GameObject.Find(POSITION_MARKER_MIN).transform.position.y;
        _maxPositionY = GameObject.Find(POSITION_MARKER_MAX).transform.position.y;
    }

    public override void Dispose()
    {
        GameManager.Instance.OnRestartGame -= OnRestartGame;
        ZombiesList.Clear();
        base.Dispose();
    }
}
