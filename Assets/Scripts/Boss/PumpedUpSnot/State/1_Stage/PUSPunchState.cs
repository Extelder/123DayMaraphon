using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PUSPunchState : State
{
    [SerializeField] private PusAnimator _animator;
    [SerializeField] private PUSSmoothlyLookAt _smoothlyLookAt;
    [SerializeField] private LayerMask _groundCheckMask;

    [SerializeField] private Pool _groundCrackPool;
    [SerializeField] private Pool _shockWavePool;

    [SerializeField] private Transform _forwardTarget;
    [SerializeField] private Transform _leftPunchPoint;
    [SerializeField] private Transform _rightPunchPoint;

    public event Action ArmSlamed;

    public override void Enter()
    {
        _smoothlyLookAt.Target = _forwardTarget;
        _smoothlyLookAt.SetTurnSpeed(200);
        float random = Random.value;
        CanChanged = false;
        if (random < 0.5)
            _animator.LeftPunch();
        else
            _animator.RightPunch();
    }

    public void PerformLeftPunch()
    {
        PerformPunch(_leftPunchPoint);
    }

    public void PerformRightPunch()
    {
        PerformPunch(_rightPunchPoint);
    }

    public void PerformPunch(Transform groundCheckPoint)
    {
        Debug.DrawRay(groundCheckPoint.position, Vector3.down * Int16.MaxValue);
        RaycastHit hit;

        if (Physics.Raycast(groundCheckPoint.position, Vector3.down, out hit, Int16.MaxValue, _groundCheckMask))
        {
            _groundCrackPool.GetFreeElement(hit.point + new Vector3(0, 0f, 0));
            _shockWavePool.GetFreeElement(hit.point + new Vector3(0, 1f, 0));
            ArmSlamed?.Invoke();
        }
    }

    public override void Exit()
    {
        _smoothlyLookAt.ReturnToDefaultSpeed();
        _smoothlyLookAt.Target = _smoothlyLookAt.Player;
    }

    public void PunchAnimationEnd()
    {
        _smoothlyLookAt.ReturnToDefaultSpeed();
        _smoothlyLookAt.Target = _smoothlyLookAt.Player;
        CanChanged = true;
    }
}