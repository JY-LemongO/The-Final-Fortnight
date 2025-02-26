using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class SurvivorManager : SingletonBase<SurvivorManager>
{
    #region Events
    // T1: ChangedSurvivor, T2: prevCount, T3: currentCount
    public event Action<Survivor> OnSurvivorListChanged;
    #endregion

    public List<SurvivorData> SelectableSurvivorList { get; private set; } = new();
    public List<WeaponData> SelectableSurvivorsWeaponList { get; private set; } = new();

    private List<Survivor> _spawnedSurvivorList = new();
    private string _survivorPrefabKey;

    #region Temp Code
    private Vector2 _spawnPosition;
    private const string SURVIVOR_SPAWN_MARKER = "SurvivorSpawnPoint";
    #endregion

    public void SpawnSurvivorByData(int id)
    {
        SurvivorData survivorData = GetSurvivorData(id);
        
        Survivor survivor = NewSurvivor();
        survivor.SetupByData(survivorData);

        WeaponStatus craftedWeapon = WeaponManager.Instance.CraftWeaponByData(survivorData.defaultWeaponId);
        WeaponManager.Instance.EquipWeapon(survivor, craftedWeapon);

        SpawnSurvivor(survivor);
    }

    public SurvivorData GetSurvivorData(int id)
    {
        if (!DataManager.Instance.SurvivorData.TryGetValue(id, out SurvivorData data))
        {
            DebugUtility.LogError($"[SurvivorManager] {id}에 해당하는 SurvivorData가 존재하지 않습니다.");
            return null;
        }
        return data;
    }

    private void SpawnSurvivor(Survivor survivor)
    {
        RegisterSurvivor(survivor);
        OnSurvivorListChanged?.Invoke(survivor);

        // Test Code
        if (_spawnedSurvivorList.Count == 1)
            GameManager.Instance.StartGame();
    }

    public void DispawnSurvivor(Survivor dispawnSurvivor)
    {
        foreach (var survivor in _spawnedSurvivorList)
        {
            if (survivor == dispawnSurvivor)
            {
                PoolManager.Instance.Return(survivor.gameObject);
                _spawnedSurvivorList.Remove(survivor);
                OnSurvivorListChanged?.Invoke(dispawnSurvivor);
                break;
            }
        }
    }

    public SurvivorData GetSelectableSurvivor(int index)
        => SelectableSurvivorList[index];

    public List<Survivor> GetSurvivorsList()
        => _spawnedSurvivorList;

    private void InitSelectableSurvivorList()
    {
        foreach (var selectableValue in DataManager.Instance.SelectableSurvivorsData.Values)
        {
            SurvivorData survivorData = GetSurvivorData(selectableValue.Id);

            SelectableSurvivorList.Add(survivorData);
            SelectableSurvivorsWeaponList.Add(WeaponManager.Instance.GetWeaponData(survivorData.defaultWeaponId));
        }
    }

    private void InitSurvivorManage()
    {
        _survivorPrefabKey = Constants.Key_Survivor;
        _spawnedSurvivorList = new();
        // Temp
        _spawnPosition = GameObject.Find(SURVIVOR_SPAWN_MARKER).transform.position;
    }

    private Survivor NewSurvivor()
    {
        GameObject go = ResourceManager.Instance.Instantiate(_survivorPrefabKey);
        go.transform.position = _spawnPosition;
        _spawnPosition += Vector2.up * 1f;

        return go.GetComponent<Survivor>();
    }

    private void RegisterSurvivor(Survivor survivor)
        => _spawnedSurvivorList.Add(survivor);

    private void OnRestartGame()
    {
        foreach (var survivor in _spawnedSurvivorList)
        {
            survivor.ResetEntity();
            PoolManager.Instance.Return(survivor.gameObject);
        }
        _spawnedSurvivorList.Clear();
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
        _spawnedSurvivorList.Clear();
        GameManager.Instance.OnRestartGame -= OnRestartGame;
        base.Dispose();
    }
}
