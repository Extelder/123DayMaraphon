using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class WeaponStateMachine : StateMachine
{
    [field: SerializeField] public ItemTakeUp Item { get; private set; }

    [Header("States")] [SerializeField] private State _idle;
    [Inject] public PlayerInputs PlayerInputs { get; private set; }
    [SerializeField] private WeaponShootState _shoot;

    public override void OnEnable()
    {
        base.OnEnable();

        PlayerInputs.PlayerWeaponInputs.MainShootPressedDown += OnMainShootPressedDown;
        PlayerInputs.PlayerWeaponInputs.MainShootPressedUp += OnMainShootPressedUp;
    }

    private void OnMainShootPressedUp()
    {
        StopAllCoroutines();
        if (_shoot.CanShoot == false)
            ChangeState(_idle);
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
            if (_shoot.CanShoot == false)
            {
                ChangeState(_idle);
                break;
            }

            if (Item.TakeUpped && _shoot.CanShoot)
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
        PlayerInputs.PlayerWeaponInputs.MainShootPressedDown -= OnMainShootPressedDown;
        PlayerInputs.PlayerWeaponInputs.MainShootPressedUp -= OnMainShootPressedUp;
    }
}