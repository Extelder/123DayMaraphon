using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashAmount : MonoBehaviour
{
    [SerializeField] private PlayerDash _dash;
    [SerializeField] private float _costByDash;
    [SerializeField] private float _earnSpeed;
    [SerializeField] private float _delayAfterSpendToEarn = 0.1f;
    [Range(0, 1)] [SerializeField] private float _capacity;

    private float _current;

    private bool _earn = true;

    public event Action<float> AmountChanged;

    public static PlayerDashAmount Instance { get; private set; }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            return;
        }

        Debug.LogError(gameObject + "one more dashAmount");
    }

    private void OnEnable()
    {
        _dash.Dashed += OnDashed;
        _current = _capacity;
    }

    public void FixedUpdate()
    {
        if (_earn)
            Earn();
    }

    public void RecoverSpeed(float addibleSpeed)
    {
        _earn = false;
        _current += addibleSpeed;
        _earn = true;
    }

    private void Earn()
    {
        _current += _earnSpeed;

        if (_current > _capacity)
            _current = _capacity;

        AmountChanged?.Invoke(_current);
        if (_current >= _costByDash)
        {
            _dash.EnableDash();
        }
    }

    private void OnDashed()
    {
        StopAllCoroutines();

        Spend(_costByDash);

        _earn = false;

        StartCoroutine(WaitForContinueEarning());

        if (_current <= 0)
        {
            _dash.DisableDash();
        }
    }

    private void Spend(float value)
    {
        if (_current - value < 0)
        {
            _current = 0;
            AmountChanged?.Invoke(_current);
            return;
        }

        _current -= value;
        AmountChanged?.Invoke(_current);
    }

    private IEnumerator WaitForContinueEarning()
    {
        yield return new WaitForSeconds(_delayAfterSpendToEarn);
        _earn = true;
    }

    private void OnDisable()
    {
        _dash.Dashed -= OnDashed;
    }
}