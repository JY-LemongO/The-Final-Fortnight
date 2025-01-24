using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class SpawnData
{
    public string zombieKey;
    public float waitTime;
    public float spawnInterval;
    public int spawnCount;

    public SpawnData(WaveData waveData)
    {
        this.zombieKey = waveData.zombieKey;
        this.waitTime = waveData.waitTime;
        this.spawnInterval = waveData.spawnInterval;
        this.spawnCount = waveData.spawnCount;
    }
}

public class WaveManager : SingletonBase<WaveManager>
{
    public event Action<float> OnRestTimeChanged;
    public event Action<bool> OnWaveStateChanged;

    public Dictionary<int, List<SpawnData>> SpawnDataDict { get; private set; } = new();

    public bool IsWaveEnd
    {
        get => _isWaveEnd;
        set
        {
            _isWaveEnd = value;
            OnWaveStateChanged?.Invoke(value);
        }
    }

    private bool _isWaveEnd = true;
    private int _currentSpawnDataCount;
    private int _currentSpawnEndedCount;
    private int _currentWave;

    public void StartWave(int wave)
    {
        _currentWave = wave;
        StartWave();
    }    

    private void StartWave()
    {
        List<SpawnData> spawnDatas = SpawnDataDict[_currentWave];
        _currentSpawnDataCount = spawnDatas.Count;

        foreach (SpawnData spawnData in spawnDatas)
            StartCoroutine(Co_WaveStart(spawnData));
    }

    private void WaveDataSetup()
    {
        Dictionary<int, WaveData> waveData = DataManager.Instance.WaveData;

        foreach (var data in waveData.Values)
        {
            SpawnData spawnData = new SpawnData(data);
            if (!SpawnDataDict.ContainsKey(data.wave))
                SpawnDataDict.Add(data.wave, new List<SpawnData>());
            SpawnDataDict[data.wave].Add(spawnData);
        }
    }    

    protected override void InitChild()
    {
        WaveDataSetup();
    }

    private IEnumerator Co_WaveStart(SpawnData spawnData)
    {
        yield return Util.GetCachedWaitForSeconds(spawnData.waitTime);

        int currentSpawnCount = 0;
        while (currentSpawnCount < spawnData.spawnCount)
        {
            currentSpawnCount++;
            ZombieManager.Instance.SpawnZombie(spawnData.zombieKey);
            yield return Util.GetCachedWaitForSeconds(spawnData.spawnInterval);
        }

        Debug.Log($"Zombie - {spawnData.zombieKey}:: Coroutine has Ended.");

        _currentSpawnEndedCount++;
        if (_currentSpawnDataCount == _currentSpawnEndedCount)
            StartCoroutine(Co_RestForNextWave());
    }

    private IEnumerator Co_RestForNextWave()
    {
        float restTime = 10f;
        OnRestTimeChanged?.Invoke(restTime);
        _currentSpawnEndedCount = 0;

        while (restTime > 0f)
        {
            yield return Util.GetCachedWaitForSeconds(1f);
            restTime--;
            OnRestTimeChanged?.Invoke(restTime);
        }

        Debug.Log("Rest over. Next Day Start.");
        _currentWave++;
        StartWave();
    }
}