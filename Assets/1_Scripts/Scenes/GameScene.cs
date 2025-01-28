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

    #region Debug
#if UNITY_EDITOR
    [Header("Debug Only")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector3 fromLeft = new Vector3(minX, -3);
        Gizmos.DrawRay(fromLeft, Vector3.up * 6);

        Vector3 fromRight = new Vector3(maxX, -3);
        Gizmos.DrawRay(fromRight, Vector3.up * 6);

        Vector3 fromBottom = new Vector3(-20, minY);
        Gizmos.DrawRay(fromBottom, Vector3.right * 40);

        Vector3 fromTop = new Vector3(-20, maxY);        
        Gizmos.DrawRay(fromTop, Vector3.right * 40);
    }
#endif
    #endregion
}
