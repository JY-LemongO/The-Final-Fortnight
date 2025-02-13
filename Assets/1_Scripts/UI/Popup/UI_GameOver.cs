using UnityEngine;
using UnityEngine.UI;

public class UI_GameOver : UI_Popup
{    
    [SerializeField] private Button _retryBtn;    

    protected override void ButtonsAddListener()
    {
        _retryBtn.onClick.AddListener(OnRetryBtn);
    }

    private void OnRetryBtn()
    {
        GameManager.Instance.Restart();
        Close();
    }         

    protected override void Dispose()
    {
        
    }
}
