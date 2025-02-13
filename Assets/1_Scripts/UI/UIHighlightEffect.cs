using UnityEngine;
using UnityEngine.UI;

public class UIHighlightEffect : MonoBehaviour
{
    [SerializeField] private Image _highlightImage;

    public void SetHighlight(bool enable)
        => _highlightImage.gameObject.SetActive(enable);
}
