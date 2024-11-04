using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpAmount : Amount
{
    [SerializeField] private PlayerJump _jump;
    [SerializeField] private WallChecker _wallChecker;
    [SerializeField] private GroundChecker _groundChecker;

    public event Action<float> AmountChanged;

    public static PlayerJumpAmount Instance { get; private set; }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            return;
        }


        Debug.LogError(gameObject + " one more jumpAmount");
    }

    private void OnEnable()
    {
        _jump.Jumped += OnJumped;
        _groundChecker.GroundDetected += OnGroundDetected;
    }

    private void OnDisable()
    {
        _jump.Jumped -= OnJumped;
        _groundChecker.GroundDetected -= OnGroundDetected;
    }

    private void OnGroundDetected()
    {
        FullRecoverSpeed();
    }

    public void FixedUpdate()
    {
        _wallChecker.CheckForWall(out Collider[] colliders, out int size);
        for (var i = 0; i < size; i++)
        {
            var target = colliders[i].gameObject;
            if (target.TryGetComponent<Wall>(out Wall wall))
            {
                FullRecoverSpeed();
            }
        }

        if (earn)
            Earn();
    }

    public void RecoverSpeed(float addibleSpeed)
    {
        earn = false;
        current += addibleSpeed;
        if (current > capacity)
            current = capacity;
        earn = true;
    }

    public void FullRecoverSpeed()
    {
        RecoverSpeed(capacity);
    }

    private void Earn()
    {
        current += earnSpeed;

        if (current > capacity)
            current = capacity;

        AmountChanged?.Invoke(current);
        if (current >= costByMove)
        {
            _jump.EnableJump();
        }
    }

    private void OnJumped()
    {
        StopAllCoroutines();

        Spend(costByMove);

        earn = false;

        StartCoroutine(WaitForContinueEarning());

        if (current <= costByMove)
        {
            _jump.DisableJump();
        }
    }

    private void Spend(float value)
    {
        if (current - value < 0)
        {
            current = 0;
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
}