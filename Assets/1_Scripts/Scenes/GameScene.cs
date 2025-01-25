using TMPro;
using UnityEngine;

public class GameScene : BaseScene
{
    #region Test
    [SerializeField] TMP_Text _timerText;
    #endregion

    protected override void Init()
    {
        base.Init();
        CurrentScene = Define.SceneType.Game;

        UIManager.Instance.OpenPopupUI<UI_GameScene>();        
        WaveManager.Instance.OnRestTimeChanged += time => _timerText.text = $"다음 웨이브까지 : {time.ToString()}s";
    }

    public override void Dispose()
    {
        
    }
}
