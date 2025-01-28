using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TurretShootState : State
{
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _maxRandomToShoot;
    [SerializeField] private float _range = 500f;

    [Inject] private Pools _pools;

    public override void Enter()
    {
        StopAllCoroutines();
        
        StartCoroutine(WaitingForChangeState());
    }

    public override void Exit()
    {
        StopAllCoroutines();
    }

    private void OnDisable()
    {
        Exit();
    }

    private IEnumerator WaitingForChangeState()
    {
        while (true)
        {
            yield return new WaitForSeconds(_maxRandomToShoot);
            Vector3 direction = _shootPoint.position + _shootPoint.forward * _range;
            Projectile projectile = _pools.TURRETProjectilePool
                .GetFreeElement(_shootPoint.position, Quaternion.FromToRotation(_shootPoint.position, direction))
                .GetComponent<Projectile>();
            projectile.Initiate(direction);
        }
    }
}