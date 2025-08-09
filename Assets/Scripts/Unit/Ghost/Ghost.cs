using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Ghost : MonoBehaviour, IHypeMeasurable
{
    [field: SerializeField] public float HypeValue { get; set; } = 0.125f;
    [field: SerializeField] public HypeType HypeType { get; set; }

    [SerializeField] private OverlapSettings _overlapSettings;

    [SerializeField] private float _checkRate;
    [field: SerializeField] public List<IGhostTrapable> TrapedUnits { get; private set; } = new List<IGhostTrapable>();

    public int GhostRadiusMultiplier { get; set; } = 1;

    private void OnEnable()
    {
        StartCoroutine(CheckingForEnemies());
    }

    private IEnumerator CheckingForEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(_checkRate);
            OverlapSphere();
            foreach (var other in _overlapSettings.Colliders)
            {
                if (other == null)
                    continue;
                if (other.TryGetComponent<IGhostTrapable>(out IGhostTrapable trapable))
                {
                    if (TrapedUnits.Contains(trapable))
                        continue;
                    TrapedUnits.Add(trapable);
                    trapable.Trap(this);
                }
            }
        }
    }

    private void UnStunAllUnits()
    {
        foreach (var traped in TrapedUnits)
        {
            if (traped == null)
                continue;
            traped.UnTrap();
        }
    }

    private void OverlapSphere()
    {
        _overlapSettings.Colliders = new Collider[10];
        _overlapSettings.Size = Physics.OverlapSphereNonAlloc(
            _overlapSettings._overlapPoint.position + _overlapSettings._positionOffset,
            _overlapSettings._sphereRadius * GhostRadiusMultiplier, _overlapSettings.Colliders,
            _overlapSettings._searchLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_overlapSettings._overlapPoint.position, _overlapSettings._sphereRadius);
    }

    private void OnDisable()
    {
        GhostRadiusMultiplier = 1;
        transform.localEulerAngles = new Vector3(0.1f, 0.1f, 0.1f);
        UnStunAllUnits();
        TrapedUnits.Clear();
    }
}