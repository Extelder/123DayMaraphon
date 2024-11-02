using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpAmount : MonoBehaviour
{
    [SerializeField] private PlayerJump _jump;
    [SerializeField] private WallChecker _wallChecker;
    [SerializeField] private GroundChecker _groundChecker;
    [SerializeField] private float _costByJump;
    [SerializeField] private float _earnSpeed;
    [SerializeField] private float _delayAfterSpendToEarn = 0.1f;
    [Range(0, 1)] [SerializeField] private float _capacity;

    private float _current;

    private bool _earn = true;

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
        _current = _capacity;
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

        if (_earn)
            Earn();
    }

    public void RecoverSpeed(float addibleSpeed)
    {
        _earn = false;
        _current += addibleSpeed;
        if (_current > _capacity)
            _current = _capacity;
        _earn = true;
    }

    public void FullRecoverSpeed()
    {
        RecoverSpeed(_capacity);
    }

    private void Earn()
    {
        _current += _earnSpeed;

        if (_current > _capacity)
            _current = _capacity;

        AmountChanged?.Invoke(_current);
        if (_current >= _costByJump)
        {
            _jump.EnableJump();
        }
    }

    private void OnJumped()
    {
        StopAllCoroutines();

        Spend(_costByJump);

        _earn = false;

        StartCoroutine(WaitForContinueEarning());

        if (_current <= _costByJump)
        {
            _jump.DisableJump();
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
}