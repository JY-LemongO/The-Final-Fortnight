using System.Collections.Generic;
using UnityEngine;

public class SurvivorManager : SingletonBase<SurvivorManager>
{
    private Dictionary<string, Survivor> _survivorsDict = new();
    private Dictionary<string, Weapon_SO> _weaponSODict = new();

    #region Temp Code
    private Vector2 _spawnPosition;

    private const string SURVIVOR_SPAWN_MARKER = "SurvivorSpawnPoint";
    private const string KEY_SURVIVOR_PREFAB = "Survivor.prefab";
    #endregion

    protected override void InitChild()
    {
        // Temp
        _spawnPosition = GameObject.Find(SURVIVOR_SPAWN_MARKER).transform.position;
    }

    public void SpawnSurvivor(string survivorKey)
    {
        GameObject go = ResourceManager.Instance.Instantiate(KEY_SURVIVOR_PREFAB);
        go.transform.position = _spawnPosition;

        Survivor survivor = go.GetComponent<Survivor>();
        survivor.SetupEntity<Survivor_SO>(survivorKey);
    }

    public void ChangeWeapon(string selectedSurvivorKey, string weaponKey)
    {
        Weapon_SO newWeapon = null;

        if (!_survivorsDict.TryGetValue(selectedSurvivorKey, out Survivor survivor))
        {
            Debug.LogError($"{selectedSurvivorKey}를 가진 Survivor가 존재하지 않습니다.");
            return;
        }
        if (!_weaponSODict.ContainsKey(weaponKey))
        {
            Weapon_SO newWeaponOrigin = ResourceManager.Instance.Load<Weapon_SO>(weaponKey);
            newWeapon = newWeaponOrigin.Clone() as Weapon_SO;
            _weaponSODict.Add(weaponKey, newWeapon);
        }
        survivor.CurrentWeapon.ChangeWeapon(newWeapon);
    }

    public override void Dispose()
    {
        
    }
}
