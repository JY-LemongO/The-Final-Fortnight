using System;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorManager : SingletonBase<SurvivorManager>
{
    #region Events
    // T1: ChangedSurvivor, T2: prevCount, T3: currentCount
    public event Action<Survivor> OnSurvivorListChanged;
    #endregion

    public List<Survivor_SO> SelectableSurvivorList { get; private set; } = new();
    public List<Weapon_SO> SelectableSurvivorsWeaponList { get; private set; } = new();

    private List<Survivor> _spawnedSurvivorList = new();
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
        survivor.SetBulletUI();

        RegisterSurvivor(survivor);
        OnSurvivorListChanged?.Invoke(survivor);
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

    public Survivor_SO GetSelectableSurvivor(int index)
        => SelectableSurvivorList[index];

    public List<Survivor> GetSurvivorsList()
        => _spawnedSurvivorList;

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
        _spawnedSurvivorList = new();
        // Temp
        _spawnPosition = GameObject.Find(SURVIVOR_SPAWN_MARKER).transform.position;
    }

    private Survivor NewSurvivor()
    {
        GameObject go = ResourceManager.Instance.Instantiate(_survivorPrefabKey);
        go.transform.position = _spawnPosition;

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
