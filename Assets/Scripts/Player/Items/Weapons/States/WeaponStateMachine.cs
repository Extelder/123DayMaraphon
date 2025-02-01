using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class WeaponStateMachine : StateMachine
{
    [SerializeField] private bool _kuinitana;

    [field: SerializeField] public ItemTakeUp Item { get; private set; }

    [Header("States")] [SerializeField] private State _idle;
    [Inject] public PlayerInputs PlayerInputs { get; private set; }
    [SerializeField] private WeaponShootState _shoot;

    public override void OnEnable()
    {
        base.OnEnable();

        if (_kuinitana)
        {
            PlayerInputs.PlayerWeaponInputs.KunitanaShootPressedDown += OnMainShootPressedDown;
            PlayerInputs.PlayerWeaponInputs.KunitanaShootPressedUp += OnMainShootPressedUp;

            return;
        }

        if (!_kuinitana)
        {
            PlayerInputs.PlayerWeaponInputs.MainShootPressedDown += OnMainShootPressedDown;
            PlayerInputs.PlayerWeaponInputs.MainShootPressedUp += OnMainShootPressedUp;
        }
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
        if (_kuinitana)
        {
            PlayerInputs.PlayerWeaponInputs.KunitanaShootPressedDown -= OnMainShootPressedDown;
            PlayerInputs.PlayerWeaponInputs.KunitanaShootPressedUp -= OnMainShootPressedUp;
            return;
        }

        if (!_kuinitana)
        {
            PlayerInputs.PlayerWeaponInputs.MainShootPressedDown -= OnMainShootPressedDown;
            PlayerInputs.PlayerWeaponInputs.MainShootPressedUp -= OnMainShootPressedUp;
        }
    }
}