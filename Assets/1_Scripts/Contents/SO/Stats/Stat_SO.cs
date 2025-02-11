using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stat/New Stat", fileName = "STAT_")]
public class Stat_SO : BaseSO
{
    public event Action<Stat_SO, float, float> OnStatValueChanged;
    public event Action<float, float> OnStatCurrentValueChanged;

    [Header("Values")]
    [SerializeField] private bool _isConsumable;
    [SerializeField] private bool _isPercentage;
    [SerializeField] private float _minValue;
    [SerializeField] private float _maxValue;
    [SerializeField] private float _defaultValue;

    private float _currentValue;

    private Dictionary<string, float> _bonusValuesDict;

    public bool IsConsumable => _isConsumable;
    public bool IsPercentage => _isPercentage;
    public float MinValue => _minValue;
    public float MaxValue => _maxValue;
    public float DefaultValue => _defaultValue;
    public float Value => Mathf.Clamp(_defaultValue + BonusValue, MinValue, MaxValue);
    public float CurrentValue
    {
        get => _currentValue;
        private set
        {
            _currentValue = Mathf.Clamp(value, MinValue, MaxValue);
            OnStatCurrentValueChanged?.Invoke(CurrentValue, Value);
        }
    }

    public float BonusValue { get; private set; }

    public void SetBonusValue(string key, float value)
    {
        float prevValue = Value;

        _bonusValuesDict[key] = value;
        BonusValue += value;

        OnStatValueChanged?.Invoke(this, Value, prevValue);
    }

    public void RemoveBonusValue(string key, float value)
    {
        if (!_bonusValuesDict.ContainsKey(key))
        {
            Debug.LogError($"{key}에 해당하는 보너스 스탯이 없습니다.");
            return;
        }

        float prevValue = Value;
        _bonusValuesDict.Remove(key);
        BonusValue -= value;

        OnStatValueChanged?.Invoke(this, Value, prevValue);
    }

    public void Consume(float amount)
    {
        if (!IsConsumable)
            return;

        CurrentValue -= amount;
    }

    public void Recover(float amount)
    {
        if (!IsConsumable)
            return;

        CurrentValue += amount;
    }

    public void ResetCurrentValue()
        => CurrentValue = Value;
}
