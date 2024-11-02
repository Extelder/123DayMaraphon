using System;
using System.Collections;
using UnityEngine;

public class PlayerDashAmount : PlayerStaminaAmount
{
    [SerializeField] private PlayerDash _dash;

    public event Action<float> AmountChanged;

    public static PlayerDashAmount Instance { get; private set; }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            return;
        }

        Debug.LogError(gameObject + " one more dashAmount");
    }


    private void OnEnable()
    {
        _dash.Dashed += OnDashed;
    }

    private void FixedUpdate()
    {
        if (earn)
            Earn();
    }

    private void Earn()
    {
        current += earnSpeed;

        AmountChanged?.Invoke(current);
        if (current >= costByMove)
        {
            _dash.EnableDash();
        }
    }

    private void OnDashed()
    {
        StopAllCoroutines();

        Spend(costByMove);

        earn = false;

        StartCoroutine(WaitForContinueEarning());

        if (current <= costByMove)
        {
            _dash.DisableDash();
        }
    }

    public void RecoverSpeed(float addibleSpeed)
    {
        earn = false;
        current += addibleSpeed;
        earn = true;
    }

    private void Spend(float value)
    {
        if (current - value < 0)
        {
            AmountChanged?.Invoke(current);
            return;
        }

        current -= value;
        AmountChanged?.Invoke(current);
    }

    private IEnumerator WaitForContinueEarning()
    {
        yield return new WaitForSeconds(delayAfterSpendToEarn);
        earn = true;
    }

    private void OnDisable()
    {
        _dash.Dashed -= OnDashed;
    }
}