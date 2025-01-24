using System.Collections;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] AnimationCurve _curve;

    private Animator _animator;
    private TextMeshPro _damageText;

    private const float _damageLow = 10f;
    private const float _damageMed = 50f;
    private const float _disappearTime = 0.75f;
    private const float _moveUpOffsetMax = 0.5f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _damageText = GetComponentInChildren<TextMeshPro>();
    }

    public void SetDamageText(float damage)
    {
        _damageText.text = damage.ToString();
        _damageText.color = GetColorFromHexByDamage(damage);
        _damageText.fontSize = GetFontSizeByDamage(damage);
        _animator.speed = 0f;

        StartCoroutine(Co_MoveText());
    }

    private Color GetColorFromHexByDamage(float damage)
    {
        Color color = Color.white;
        string hex = Constants.DamageColorHEX_H;

        if (damage < _damageMed)
            hex = Constants.DamageColorHEX_M;
        if (damage < _damageLow)
            hex = Constants.DamageColorHEX_L;

        if (ColorUtility.TryParseHtmlString(hex, out Color hexToColor))
            color = hexToColor;

        return color;
    }

    private float GetFontSizeByDamage(float damage)
    {
        float fontSize = Constants.DamageTextFontSize_H;

        if (damage < _damageMed)
            fontSize = Constants.DamageTextFontSize_M;
        if (damage < _damageLow)
            fontSize = Constants.DamageTextFontSize_L;

        return fontSize;
    }

    private void HandleDisappear()
        => PoolManager.Instance.Return(gameObject);

    private IEnumerator Co_MoveText()
    {
        float current = 0f;
        float percent = 0f;
        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + Vector2.up * _moveUpOffsetMax;

        while (percent < 1f)
        {
            current += Time.deltaTime;
            percent = current / _disappearTime;

            transform.position = Vector3.Lerp(startPosition, endPosition, _curve.Evaluate(percent));
            yield return null;
        }

        transform.position = endPosition;
        _animator.speed = 1f;
    }
}
