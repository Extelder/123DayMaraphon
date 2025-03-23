using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUSNearbyCheck : MonoBehaviour
{
    [SerializeField] private OverlapSettings _overlapSettings;
    [SerializeField] private float _checkRate;
    [SerializeField] private Animator _animator;

    private void OnEnable()
    {
        StartCoroutine(Checking());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_overlapSettings._overlapPoint.position, _overlapSettings._sphereRadius);
    }

    private IEnumerator Checking()
    {
        while (true)
        {
            yield return new WaitForSeconds(_checkRate);

            OverlapSphere();
            foreach (var other in _overlapSettings.Colliders)
            {
                if (other == null)
                    continue;
                if (other.TryGetComponent<PlayerHitBox>(out PlayerHitBox PlayerHitBox))
                {
                    _animator.SetTrigger("Eat");
                    yield break;
                }
            }
        }
    }


    private void OverlapSphere()
    {
        _overlapSettings.Colliders = new Collider[10];
        _overlapSettings.Size = Physics.OverlapSphereNonAlloc(
            _overlapSettings._overlapPoint.position + _overlapSettings._positionOffset,
            _overlapSettings._sphereRadius, _overlapSettings.Colliders,
            _overlapSettings._searchLayer);
    }
}