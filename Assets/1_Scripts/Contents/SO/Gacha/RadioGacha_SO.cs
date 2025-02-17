using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SurvivorGachaDataBase")]
public class RadioGacha_SO : BaseSO
{
    [field:SerializeField] public List<GachaEntry> GachaEntries { get; private set; }

    public Survivor_SO GetRandomSurvivor()
    {
        float weight = 0f;



        return null;
    }
}

[Serializable]
public class GachaEntry
{
    public Survivor_SO survivorSO;
    public float weight;
}

