using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName ="SurvivorGachaDataBase")]
public class RadioGacha_SO : BaseSO
{
    [field:SerializeField] public List<GachaEntry> GachaEntries { get; private set; }

    public Survivor_SO GetRandomSurvivor()
    {
        float totalWeight = 0f;

        foreach(var entry in GachaEntries)
            totalWeight += entry.weight;

        float currentSum = 0f;
        float randomPercentage = Random.Range(0f, totalWeight);

        foreach (var entry in GachaEntries)
        {
            currentSum += entry.weight;
            if (currentSum <= randomPercentage)
                return entry.survivorSO;
        }

        DebugUtility.LogError("가챠 가중치 범위내에 해당하는 Survivor가 없음.");
        return null;
    }
}

[Serializable]
public class GachaEntry
{
    public Survivor_SO survivorSO;
    public float weight;
}

