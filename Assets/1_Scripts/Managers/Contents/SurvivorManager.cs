using System;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

public class SurvivorManager : SingletonBase<SurvivorManager>
{
    #region Events
    // T1: ChangedSurvivor, T2: prevCount, T3: currentCount
    public event Action<Survivor, int, int> OnSurvivorsCountChanged;
    #endregion

    public List<Survivor_SO> SelectableSurvivorList { get; private set; } = new();
    public List<Weapon_SO> SelectableSurvivorsWeaponList { get; private set; } = new();

    private Dictionary<string, List<Survivor>> _spawnedSurvivorDict = new();
    private string _survivorPrefabKey;

    #region Temp Code
    private Vector2 _spawnPosition;
    private const string SURVIVOR_SPAWN_MARKER = "SurvivorSpawnPoint";
    #endregion

    public void SpawnSurvivor(string survivorSOKey)
    {
        Survivor_SO survivorSO = ResourceManager.Instance.Load<Survivor_SO>(survivorSOKey);
        Survivor survivor = NewSurvivor();
        survivor.Setup(survivorSO);
        
        WeaponStatus craftedWeapon = WeaponManager.Instance.CraftWeapon(survivorSO.DefaultWeapon);
        survivor.SetWeapon(craftedWeapon);

        int prevSurvivorsCount = GetSurvivorsCount();
        RegisterSurvivor(survivor);
        OnSurvivorsCountChanged?.Invoke(survivor, prevSurvivorsCount, GetSurvivorsCount());
    }

    private Survivor NewSurvivor()
    {
        GameObject go = ResourceManager.Instance.Instantiate(_survivorPrefabKey);
        go.transform.position = _spawnPosition;

        return go.GetComponent<Survivor>();
    }

    public void DispawnSurvivor(Survivor dispawnSurvivor)
    {
        int prevSurvivorsCount = GetSurvivorsCount();
        string key = dispawnSurvivor.SurvivorStatus.CodeName;

        if (!_spawnedSurvivorDict.ContainsKey(key))
            return;

        foreach (var survivor in _spawnedSurvivorDict[key])
        {
            if (survivor == dispawnSurvivor)
            {
                PoolManager.Instance.Return(survivor.gameObject);
                _spawnedSurvivorDict[key].Remove(survivor);
                OnSurvivorsCountChanged?.Invoke(dispawnSurvivor, prevSurvivorsCount, GetSurvivorsCount());
                break;
            }
        }
    }

    public Survivor_SO GetSelectableSurvivor(int index)
        => SelectableSurvivorList[index];

    private void InitSelectableSurvivorList()
    {
        string[] survivorSOKeys = Enum.GetNames(typeof(Define.SurvivorKeys));

        foreach (var key in survivorSOKeys)
        {
            Survivor_SO survivorRO = ResourceManager.Instance.Load<Survivor_SO>(key);

            SelectableSurvivorList.Add(survivorRO);
            SelectableSurvivorsWeaponList.Add(survivorRO.DefaultWeapon);
        }
    }

    private void InitSurvivorManage()
    {
        _survivorPrefabKey = Constants.Key_Survivor;
        _spawnedSurvivorDict = new();
        // Temp
        _spawnPosition = GameObject.Find(SURVIVOR_SPAWN_MARKER).transform.position;
    }

    private void RegisterSurvivor(Survivor survivor)
    {
        string key = survivor.SurvivorStatus.CodeName;

        if (!_spawnedSurvivorDict.ContainsKey(key))
            _spawnedSurvivorDict.Add(key, new List<Survivor>());
        _spawnedSurvivorDict[key].Add(survivor);
    }

    private void OnRestartGame()
    {
        foreach (var survivorList in _spawnedSurvivorDict.Values)
        {
            foreach (var survivor in survivorList)
            {
                survivor.ResetEntity();
                PoolManager.Instance.Return(survivor.gameObject);
            }
        }
        _spawnedSurvivorDict.Clear();
    }

    private int GetSurvivorsCount()
    {
        int count = 0;
        foreach (var list in _spawnedSurvivorDict.Values)
            count += list.Count;
        return count;
    }

    protected override void InitChild()
    {
        InitSelectableSurvivorList();
        InitSurvivorManage();
        GameManager.Instance.OnRestartGame += OnRestartGame;
    }

    public override void Dispose()
    {
        SelectableSurvivorList.Clear();
        SelectableSurvivorsWeaponList.Clear();
        _spawnedSurvivorDict.Clear();
        GameManager.Instance.OnRestartGame -= OnRestartGame;
        base.Dispose();
    }
}
