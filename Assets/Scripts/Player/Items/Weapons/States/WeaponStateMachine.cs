using System.Collections;
using UnityEngine;
using Zenject;

public class WeaponStateMachine : StateMachine
{
    [SerializeField] private ItemTakeUp _item;

    [Header("States")] [SerializeField] private State _idle;
    [Inject] private PlayerInputs _playerInputs;
    [SerializeField] private WeaponShootState _shoot;

    public override void OnEnable()
    {
        base.OnEnable();

        _playerInputs.MainShootPressedDown += OnMainShootPressedDown;
        _playerInputs.MainShootPressedUp += OnMainShootPressedUp;
    }

    private void OnMainShootPressedUp()
    {
        StopAllCoroutines();
        StartCoroutine(TryingToExitShoot());
    }

    private void OnMainShootPressedDown()
    {
        StopAllCoroutines();
        if (_item.TakeUpped)
            ChangeState(_shoot);
    }

    private IEnumerator TryingToExitShoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.02f);
            ChangeState(_idle);
        }
    }

    private void OnDisable()
    {
        _playerInputs.MainShootPressedDown -= OnMainShootPressedDown;
        _playerInputs.MainShootPressedUp -= OnMainShootPressedUp;
    }
}