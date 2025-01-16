using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UIBase
{
    [SerializeField] Slider _hpSlider;    

    private Animator _animator;
    private Entity _entity;    

    private bool _isDisappearing = true;
    private float _currentFadeTimer = 0f;

    private const string DISAPPEAR_STATE = "HPBar_Disappear";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        if (_entity != null)
            transform.position = _entity.transform.position + _entity.HPBarPosition;
    }

    public void SetEntity(Entity entity)
    {
        if (_entity != null)
            return;

        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        _entity = entity;
        _entity.CurrentEntitySO.Hp.OnStatCurrentValueChanged += HandleHPSliderValueChange;
    }

    private void HandleHPSliderValueChange(float currentValue, float totalValue)
    {
        if (Mathf.Approximately(currentValue, totalValue))
            return;        
        
        _hpSlider.value = currentValue / totalValue;
        _currentFadeTimer = 0f;

        if (_isDisappearing)
        {
            _animator.Play(DISAPPEAR_STATE, 0, 0f);
            StartCoroutine(Co_FadeOut());
        }
    }

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

    public void ReturnToPool()
    {
        StopAllCoroutines();
        _isDisappearing = true;
        _entity.CurrentEntitySO.Hp.OnStatCurrentValueChanged -= HandleHPSliderValueChange;
        _entity = null;
        PoolManager.Instance.Return(gameObject);
    }
}
