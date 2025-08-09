using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class KunitanStateMachine : StateMachine
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _attackVariantIntAnimatorName = "AttackVariant";

    [SerializeField] private KunitanShoot _kunitanaShoot;
    [field: SerializeField] public ItemTakeUp Item { get; private set; }

    [Header("States")] [SerializeField] private State _idle;
    [SerializeField] private State _harakiri;
    [Inject] public PlayerInputs PlayerInputs { get; private set; }
    [SerializeField] private WeaponShootState _shoot;

    public override void OnEnable()
    {
        base.OnEnable();

        PlayerInputs.PlayerWeaponInputs.KunitanaShootPressedDown += OnMainShootPressedDown;
        PlayerInputs.PlayerWeaponInputs.KunitanaShootPressedUp += OnMainShootPressedUp;
    }

    private void Update()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            HarakiriState();
        }
    }

    public void Idle()
    {
        ChangeState(_idle);
    }

    public void HarakiriState()
    {
        ChangeState(_harakiri);
    }

    private void OnMainShootPressedUp()
    {
        StopAllCoroutines();
        StartCoroutine(TryingToExitShoot());
    }

    private void OnMainShootPressedDown()
    {
        StopAllCoroutines();

        StartCoroutine(WaitingForTakeUpToShoot());
    }

    private IEnumerator WaitingForTakeUpToShoot()
    {
        while (true)
        {
            if (_kunitanaShoot.IsLastShooted())
            {
                _animator.SetInteger(_attackVariantIntAnimatorName, 0);
            }
            else
            {
                _animator.SetInteger(_attackVariantIntAnimatorName, 1);
            }

            if (Item.TakeUpped)
                ChangeState(_shoot);
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator TryingToExitShoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.02f);
            ChangeState(_idle);
        }
    }

    public virtual void OnDisable()
    {
        PlayerInputs.PlayerWeaponInputs.KunitanaShootPressedDown -= OnMainShootPressedDown;
        PlayerInputs.PlayerWeaponInputs.KunitanaShootPressedUp -= OnMainShootPressedUp;
    }
}