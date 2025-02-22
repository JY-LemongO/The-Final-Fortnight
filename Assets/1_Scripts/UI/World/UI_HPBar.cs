using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_World
{
    [SerializeField] Slider _hpSlider;

    private Animator _animator;
    private Entity _entity;

    private bool _isDisappearing = true;
    private float _currentFadeTimer = 0f;

    private const string DISAPPEAR_STATE = "HPBar_Disappear";

    protected override void Init()
    {
        base.Init();

        _animator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        if (_entity != null)
            transform.position = _entity.transform.position + Vector3.up * _entity.Status.HPBarOffset;
    }

    public void SetEntity(Entity entity)
    {
        if (_entity != null)
            return;

        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        _entity = entity;
        _entity.Status.OnHPValueChanged += HandleHPSliderValueChange;
        _entity.Status.OnDead += OnDisappearImmediately;
    }

    public void SetHPBarWidth(float width)
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }

    private void HandleHPSliderValueChange(float currentValue, float totalValue)
    {
        if (Mathf.Approximately(currentValue, totalValue))
            return;

        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        _hpSlider.value = currentValue / totalValue;
        _currentFadeTimer = 0f;

        if (_isDisappearing)
        {
            _animator.Play(DISAPPEAR_STATE, 0, 0f);
            StartCoroutine(Co_FadeOut());
        }
    }

    private void OnDisappearImmediately()
    {
        _isDisappearing = false;
        HandleDisappear();
    }

    private void HandleDisappear()
        => gameObject.SetActive(false);    

    private IEnumerator Co_FadeOut()
    {
        _animator.speed = 0f;
        _isDisappearing = false;
        float waitTime = Constants.HPBarFadeOutWaitTime;

        while (_currentFadeTimer < waitTime)
        {
            _currentFadeTimer += Time.deltaTime;
            yield return null;
        }

        _animator.speed = 1f;
        _isDisappearing = true;
    }

    protected override void Dispose()
    {
        if (!gameObject.activeSelf)
            return;

        StopAllCoroutines();
        _isDisappearing = true;
        _entity.Status.OnHPValueChanged -= HandleHPSliderValueChange;
        _entity.Status.OnDead -= OnDisappearImmediately;
        _entity = null;
        base.Dispose();
    }
}
