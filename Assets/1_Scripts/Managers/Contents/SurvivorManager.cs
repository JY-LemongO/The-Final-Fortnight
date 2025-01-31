using System;
using System.Collections.Generic;
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
        int prevSurvivorsCount = GetSurvivorsCount();

        GameObject go = ResourceManager.Instance.Instantiate(_survivorPrefabKey);
        go.transform.position = _spawnPosition;

        Survivor survivor = go.GetComponent<Survivor>();
        survivor.SetupEntity<Survivor_SO>(survivorSOKey);

        string survivorCodeName = survivor.CurrentEntitySO.CodeName;
        if (!_spawnedSurvivorDict.ContainsKey(survivorCodeName))
            _spawnedSurvivorDict.Add(survivorCodeName, new List<Survivor>());
        _spawnedSurvivorDict[survivorCodeName].Add(survivor);

        OnSurvivorsCountChanged?.Invoke(survivor, prevSurvivorsCount, GetSurvivorsCount());
    }

    public void DispawnSurvivor(Survivor dispawnSurvivor)
    {
        int prevSurvivorsCount = GetSurvivorsCount();
        string key = dispawnSurvivor.CurrentEntitySO.CodeName;

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

    public void UpgradeWeapon(string weaponSOKey)
    {

    }

    public Survivor_SO GetSelectableSurvivor(int index)
        => SelectableSurvivorList[index];

    private void InitSelectableSurvivorList()
    {
        string[] survivorSOKeys = Enum.GetNames(typeof(Define.SurvivorKeys));

        foreach (var key in survivorSOKeys)
        {            
            Survivor_SO survivoRO = ResourceManager.Instance.Load<Survivor_SO>(key);

            Survivor_SO survivor = survivoRO.Clone() as Survivor_SO;
            Weapon_SO weapon = survivoRO.DefaultWeapon.Clone() as Weapon_SO;

            SelectableSurvivorList.Add(survivor);
            SelectableSurvivorsWeaponList.Add(weapon);
        }
    }

    private void InitSurvivorManage()
    {
        _survivorPrefabKey = Constants.Key_Survivor;
        _spawnedSurvivorDict = new();
        // Temp
        _spawnPosition = GameObject.Find(SURVIVOR_SPAWN_MARKER).transform.position;
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
