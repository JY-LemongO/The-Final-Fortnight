using System.Collections;
using System.Collections.Generic;
using Data;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// 매번 로직 수정
public class AnythingTest : MonoBehaviour
{
    [SerializeField] private Survivor survivor;

    private async void Start()
    {
        await DataManager.Instance.DataInit();
        Dictionary<int, WaveData> data = DataManager.Instance.WaveData;        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //survivor.Weapon.Fire();
        }
    }
}

