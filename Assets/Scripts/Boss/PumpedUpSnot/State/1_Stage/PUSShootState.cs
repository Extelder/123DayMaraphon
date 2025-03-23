using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUSShootState : State
{
    [SerializeField] private PusAnimator _animator;
    [SerializeField] private Pool _projectilePool;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _shootRate;
    [SerializeField] private float _shootRange = 10000;

    public override void Enter()
    {
        StopAllCoroutines();

        CanChanged = false;
        Debug.LogError(CanChanged);

        _animator.Shoot();
    }

    public override void Exit()
    {
        StopAllCoroutines();
        CanChanged = true;
        Debug.LogError(CanChanged);
    }

    public void StartShooting()
    {
        StopAllCoroutines();
        StartCoroutine(Shooting());
    }

    public void StopShooting()
    {
        CanChanged = true;
        Debug.LogError(CanChanged);
        StopAllCoroutines();
    }

    private IEnumerator Shooting()
    {
        while (true)
        {
            Vector3 direction = _shootPoint.position + _shootPoint.forward * _shootRange;
            SlashProjectile slashProjectile = _projectilePool
                .GetFreeElement(_shootPoint.position, Quaternion.FromToRotation(_shootPoint.position, direction))
                .GetComponent<SlashProjectile>();

            slashProjectile.Initiate(direction);
            yield return new WaitForSeconds(_shootRate);
        }
    }
}