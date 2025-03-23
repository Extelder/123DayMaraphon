using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUSShootFromUpState : State
{
    [SerializeField] private PusAnimator _animator;
    [SerializeField] private Pool _projectilePool;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _shootRate;
    [SerializeField] private float _shootRange = 10000;
    [SerializeField] private PUSSmoothlyLookAt _smoothlyLookAt;
    [SerializeField] private Transform _forwardTarget;
    [SerializeField] private Transform[] _spawnPostions;

    public override void Enter()
    {
        StopAllCoroutines();
        _smoothlyLookAt.Target = _forwardTarget;
        _smoothlyLookAt.SetTurnSpeed(200);
        CanChanged = false;
        Debug.LogError(CanChanged);

        _animator.ShootUp();
    }

    public override void Exit()
    {
        StopAllCoroutines();
        CanChanged = true;
        Debug.LogError(CanChanged);
        _smoothlyLookAt.ReturnToDefaultSpeed();
        _smoothlyLookAt.Target = _smoothlyLookAt.Player;
    }

    public void StartShootingFromUp()
    {
        StopAllCoroutines();
        StartCoroutine(ShootingUp());
    }

    public void StopShootingFromUp()
    {
        CanChanged = true;
        Debug.LogError(CanChanged);
        StopAllCoroutines();
        _smoothlyLookAt.ReturnToDefaultSpeed();
        _smoothlyLookAt.Target = _smoothlyLookAt.Player;
    }

    private IEnumerator ShootingUp()
    {
        while (true)
        {
            IceProjectile iceProjectile = _projectilePool
                .GetFreeElement(_spawnPostions[Random.Range(0, _spawnPostions.Length)].position)
                .GetComponent<IceProjectile>();
            yield return new WaitForSeconds(_shootRate);
        }
    }
}