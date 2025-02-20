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

public interface IConvertRowData<TData>
{
    int Id { get; }
    TData ConvertRow(List<string> row);
}

public class DataManager : SingletonBase<DataManager>
{
    public Dictionary<int, WaveData> WaveData = new();
    public Dictionary<int, GachaData> GachaData = new();

    public async Task DataInit()
    {
        WaveData = await LoadJson<GenericLoader<WaveData>, int, WaveData>("WaveData!A2:F");
        GachaData = await LoadJson<GenericLoader<GachaData>, int, GachaData>("GachaData!A2:D");

        Debug.Log("Data Init Completed.");
    }

    private async Task<Dictionary<TKey, TValue>> LoadJson<TLoader, TKey, TValue>(string sheetRange)
        where TLoader : ILoader<TKey, TValue>, new()
        where TValue : IConvertRowData<TValue>, new()
    {
        string json = await SpreadSheetDataLoader.LoadSheetData(sheetRange);
        SpreadSheetData sheetData = JsonConvert.DeserializeObject<SpreadSheetData>(json);

        List<TValue> valueList = new();
        foreach (var data in sheetData.values)
        {
            TValue value = new TValue().ConvertRow(data);
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
