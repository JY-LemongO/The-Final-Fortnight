using System;
using UnityEngine;

#region InGameData[일회성]
public class InGameData
{
    #region 게임 진행
    public int Day;    
    #endregion

    #region 인게임 재화
    public int Money;
    public int Scrap;
    public int Battery;
    #endregion

    #region 업적 관련
    public int ZombieKillAmount;
    public int SurvivorFoundAmount;
    public int SurvivorLostAmount;
    public int UsedScrapAmount;
    public int UsedMoneyAmount;
    public float AliveTime;
    #endregion
}
#endregion

#region GameManager
public class GameManager : SingletonBase<GameManager>
{
    #region Events
    public event Action<int> OnDayChanged;
    public event Action<int> OnMoneyChanged;
    public event Action<int> OnScrapChanged;
    public event Action<int> OnBatteryChanged;
    public event Action OnRestartGame;
    #endregion

    private InGameData _inGameData = new();

    public int CurrentDay
    {
        get => _inGameData.Day;
        private set
        {
            _inGameData.Day = value;
            OnDayChanged?.Invoke(value);
        }
    }

    public int CurrentMoney
    {
        get => _inGameData.Money;
        private set
        {
            _inGameData.Money = value;
            OnMoneyChanged?.Invoke(value);
        }
    }

    public int CurrentScrap
    {
        get => _inGameData.Scrap;
        private set
        {
            _inGameData.Scrap = value;
            OnScrapChanged?.Invoke(value);
        }
    }

    public int CurrentBattery
    {
        get => _inGameData.Battery;
        private set
        {
            _inGameData.Battery = value;
            OnBatteryChanged?.Invoke(value);
        }
    }

    public void StartGame()
    {        
        WaveManager.Instance.StartWave(1);
    }

    public void ResumeGame()
    {
        // 저장된 데이터를 받아서 해당 웨이브 부터 시작
        WaveManager.Instance.StartWave(1);
    }

    public void GameOver()
    {
        Debug.Log("게임 오버.");
        UIManager.Instance.OpenPopupUI<UI_GameOver>();
    }

    public void Restart()
    {
        CurrentDay = 1;
        CurrentMoney = 0;
        CurrentScrap = 0;
        CurrentBattery = 0;

        OnRestartGame?.Invoke();        
    }

    protected override void InitChild()
    {

    }

    public override void Dispose()
    {
        OnDayChanged = null;
        OnMoneyChanged = null;
        OnScrapChanged = null;
        OnBatteryChanged = null;
        OnRestartGame = null;

        base.Dispose();
    }
}
#endregion
