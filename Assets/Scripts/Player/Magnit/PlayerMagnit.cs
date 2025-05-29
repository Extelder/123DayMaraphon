using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagnit : MonoBehaviour
{
    [SerializeField] private OverlapSettings _overlapSettings;
    [SerializeField] private float _checkRate;

    [SerializeField] private Transform _player;

    private void Start()
    {
        StartCoroutine(CheckingForPickupable());
    }

    private IEnumerator CheckingForPickupable()
    {
        while (true)
        {
            yield return new WaitForSeconds(_checkRate);
            OverlapSphere();

            for (int i = 0; i < _overlapSettings.Colliders.Length; i++)
            {
                if (_overlapSettings.Colliders[i] == null)
                    continue;
                if (_overlapSettings.Colliders[i].TryGetComponent<Magnitable>(out Magnitable magnitable))
                {
                    if (magnitable.Moving)
                        continue;
                    magnitable.Magnit(_player);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_overlapSettings._overlapPoint.position, _overlapSettings._sphereRadius);
    }

    private void OverlapSphere()
    {
        _overlapSettings.Colliders = new Collider[20];
        _overlapSettings.Size = Physics.OverlapSphereNonAlloc(
            _overlapSettings._overlapPoint.position + _overlapSettings._positionOffset,
            _overlapSettings._sphereRadius, _overlapSettings.Colliders,
            _overlapSettings._searchLayer);
    }
}