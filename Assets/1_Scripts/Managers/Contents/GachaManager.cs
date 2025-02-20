using System;
using System.Collections.Generic;
using Data;
using Random = UnityEngine.Random;

public class GachaManager : SingletonBase<GachaManager>
{
    #region Events
    public event Action<Survivor_SO> OnGachaResult;
    public event Action<int> OnExecuteGacha;    
    public event Action OnBatteryInSufficient;    
    #endregion

    public List<GachaData> GachaDatas { get; private set; } = new();

    public int ExecuteRequirement
    {
        get => _executeRequirement;
        private set
        {
            _executeRequirement = value;
            OnExecuteGacha?.Invoke(_executeRequirement);
        }
    }

    private int _executeRequirement;
    private int _rerollRequirement = 1;

    private Survivor_SO GetRandomSurvivor()
    {
        float totalWeight = 0f;

        foreach (var data in GachaDatas)
            totalWeight += data.weight;

        float currentSum = 0f;
        float randomPercentage = Random.Range(0f, totalWeight);

        foreach (var entry in GachaDatas)
        {
            currentSum += entry.weight;
            if (randomPercentage <= currentSum)
                return GetSurvivor(entry.survivorKey);
        }

        DebugUtility.LogError("가챠 가중치 범위내에 해당하는 Survivor가 없음.");
        return null;
    }

    public void Execute()
    {
        if (!GameManager.Instance.UseBattery(ExecuteRequirement))
        {
            DebugUtility.Log("배터리가 부족합니다.");
            OnBatteryInSufficient?.Invoke();
            return;
        }
        UIManager.Instance.OpenPopupUI<UI_FindSurvivor>();        
        ExecuteRequirement += Constants.GachaCostIncrease;
        OnGachaResult?.Invoke(GetRandomSurvivor());
    }

    public void Reroll()
    {
        if (!GameManager.Instance.UseBattery(_rerollRequirement))
        {
            DebugUtility.Log("배터리가 부족합니다.");
            OnBatteryInSufficient?.Invoke();
            return;
        }        
        OnGachaResult?.Invoke(GetRandomSurvivor());        
    }

    private Survivor_SO GetSurvivor(string key)
        => ResourceManager.Instance.Load<Survivor_SO>(key);

    protected override void InitChild()
    {
        foreach (var data in DataManager.Instance.GachaData.Values)
            GachaDatas.Add(data);

        ExecuteRequirement = Constants.ExecuteGachaInitCost;
    }

    public override void Dispose()
    {
        GachaDatas.Clear();
        OnGachaResult = null;
        OnBatteryInSufficient = null;
        OnExecuteGacha = null;
        base.Dispose();
    }
}
