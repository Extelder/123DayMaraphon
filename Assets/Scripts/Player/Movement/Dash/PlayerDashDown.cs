using System;
using System.Collections;
using UniRx;
using UnityEngine;
using Zenject;

public class PlayerDashDown : Dashing
{
    [Header("Settings")] [SerializeField] private float _dashDownCooldown;
    [Inject] private Pools _pools;


    [Space(10)] [Header("Check Floor")] [SerializeField]
    private float _checkFloorRadius;

    [SerializeField] private Transform _checkPoint;
    [SerializeField] private float _checkRate;
    [SerializeField] private LayerMask _checkLayer;
    [SerializeField] private int _maxObjectsToCheck;
    [SerializeField] private GroundChecker _groundChecker;

    private CompositeDisposable _dashDownDisposable = new CompositeDisposable();

    private Collider[] _others;

    public void DashDown()
    {
        if(_groundChecker.Detected) 
            return;

        if (!cooldownRecovered)
            return;

        StopAllCoroutines();

        Vector3 forceToApply = (orientation.up * -dashUpwardForce);
        float cooldown = _dashDownCooldown;

        AddImpulse(forceToApply, cooldown, _dashDownDisposable);
        StartCoroutine(CheckingForFloor());
    }

    private void StashedOnFloor()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1000000f, _checkLayer))
            _pools.DashDownPool.GetFreeElement(hit.point);
        StopCoroutine(LerpSpeedCoroutine);
    }

    private IEnumerator CheckingForFloor()
    {
        while (true)
        {
            yield return new WaitForSeconds(_checkRate);
            _others = new Collider[_maxObjectsToCheck];
            Physics.OverlapSphereNonAlloc(_checkPoint.position, _checkFloorRadius, _others, _checkLayer);
            foreach (var other in _others)
            {
                if (other == null)
                    continue;

                if (other.TryGetComponent<Ground>(out Ground ground))
                {
                    StashedOnFloor();
                    yield break;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_checkPoint.position, _checkFloorRadius);
    }

    private void OnDisable()
    {
        _dashDownDisposable.Clear();
    }
}