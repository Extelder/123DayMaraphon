using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private OverlapSettings _overlapSettings;

    [SerializeField] private float _checkRate;
    public List<IGhostTrapable> TrapedUnits { get; private set; } = new List<IGhostTrapable>();

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
                if(other == null)
                    continue;
                if(other.TryGetComponent<IGhostTrapable> (out IGhostTrapable trapable))
                {
                    if (TrapedUnits.Contains(trapable))
                        continue;
                    TrapedUnits.Add(trapable);
                    trapable.Trap();
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
            _overlapSettings._sphereRadius, _overlapSettings.Colliders,
            _overlapSettings._searchLayer);
    }

    private void OnDisable()
    {
        UnStunAllUnits();
    }
}
