using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PUSSlamState : State
{
    [SerializeField] private PusAnimator _animator;
    [SerializeField] private PUSSmoothlyLookAt _smoothlyLookAt;
    [SerializeField] private LayerMask _groundCheckMask;

    [SerializeField] private Pool _groundCrackPool;
    [SerializeField] private Pool _shockWavePool;

    [SerializeField] private Transform _forwardTarget;
    [SerializeField] private Transform _slamPoint;

    public event Action Slamed;

    public override void Enter()
    {
        _smoothlyLookAt.Target = _forwardTarget;
        _smoothlyLookAt.SetTurnSpeed(200);
        CanChanged = false;

        _animator.Slam();
    }

    public void PerformSlam()
    {
        PerformSlam(_slamPoint);
    }

    public void PerformSlam(Transform groundCheckPoint)
    {
        Debug.DrawRay(groundCheckPoint.position, Vector3.down * Int16.MaxValue);
        RaycastHit hit;

        if (Physics.Raycast(groundCheckPoint.position, Vector3.down, out hit, Int16.MaxValue, _groundCheckMask))
        {
            _groundCrackPool.GetFreeElement(hit.point + new Vector3(0, 0f, 0));
            _shockWavePool.GetFreeElement(hit.point + new Vector3(0, 1f, 0));
            Slamed?.Invoke();
        }
    }

    public override void Exit()
    {
        CanChanged = true;

        _smoothlyLookAt.ReturnToDefaultSpeed();
        _smoothlyLookAt.Target = _smoothlyLookAt.Player;
    }

    public void SlamAnimationEnd()
    {
        _smoothlyLookAt.ReturnToDefaultSpeed();
        _smoothlyLookAt.Target = _smoothlyLookAt.Player;
        CanChanged = true;
    }
}