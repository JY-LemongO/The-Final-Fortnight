using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class SpreadSheetDataLoader
{
    private const string API_KEY = "AIzaSyBBtEhNLdEvSvx2_wSNwoV7BV6YkP1RjH0";
    private const string SHEET_ID = "1Ot8vGWGFYVU1phuEntJdd8pgGa7KeFUmoaZEEWdUZPA";    

    public static async Task<string> LoadSheetData(string sheetRange)
    {
        string sheetUrl = $"https://sheets.googleapis.com/v4/spreadsheets/{SHEET_ID}/values/{sheetRange}?key={API_KEY}";        

        using(UnityWebRequest request = UnityWebRequest.Get(sheetUrl))
        {
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("데이터 로드 성공");
                return request.downloadHandler.text;
            }
            else
                throw new Exception($"데이터 로드 실패::{request.error}");
        }        
    }
}
