using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Newtonsoft.Json;
using UnityEngine;

public interface ILoader<TKey, TValue>
{
    Dictionary<TKey, TValue> MakeDict();
}

public interface IConvertRowData
{
    int Id { get; }
    void ConvertRow(List<string> row);
}

public class DataManager : SingletonBase<DataManager>
{
    public Dictionary<int, SurvivorData> SurvivorData { get; private set; } = new();
    public Dictionary<int, ZombieData> ZombieData { get; private set; } = new();
    public Dictionary<int, WeaponData> WeaponData { get; private set; } = new();
    public Dictionary<int, StructureData> StructureData { get; private set; } = new();
    public Dictionary<int, WaveData> WaveData { get; private set; } = new();
    public Dictionary<int, GachaData> GachaData { get; private set; } = new();    
    
    public async Task DataInit()
    {
        SurvivorData = await LoadJson<GenericLoader<SurvivorData>, int, SurvivorData>("SurvivorData!A2:J");
        ZombieData = await LoadJson<GenericLoader<ZombieData>, int, ZombieData>("ZombieData!A2:L");
        WeaponData = await LoadJson<GenericLoader<WeaponData>, int, WeaponData>("WeaponData!A2:N");
        StructureData = await LoadJson<GenericLoader<StructureData>, int, StructureData>("StructureData!A2:H");        
        WaveData = await LoadJson<GenericLoader<WaveData>, int, WaveData>("WaveData!A2:F");
        GachaData = await LoadJson<GenericLoader<GachaData>, int, GachaData>("GachaData!A2:D");

        Debug.Log("Data Init Completed.");
    }

    private async Task<Dictionary<TKey, TValue>> LoadJson<TLoader, TKey, TValue>(string sheetRange)
        where TLoader : ILoader<TKey, TValue>, new()
        where TValue : IConvertRowData, new()
    {
        string json = await SpreadSheetDataLoader.LoadSheetData(sheetRange);
        SpreadSheetData sheetData = JsonConvert.DeserializeObject<SpreadSheetData>(json);

        List<TValue> valueList = new();
        foreach (var data in sheetData.values)
        {
            TValue value = new TValue();
            value.ConvertRow(data);
            valueList.Add(value);
        }

        TLoader loader = new();
        if (loader is GenericLoader<TValue> genericLoader)
            genericLoader.rows = valueList;

        return loader.MakeDict();
    }

    protected override void InitChild() { }

    public override void Dispose()
    {
        WaveData.Clear();
        base.Dispose();
    }
}

[Serializable]
public class SpreadSheetData
{
    public string sheetRange;
    public string majorDimension;
    public List<List<string>> values;
}
