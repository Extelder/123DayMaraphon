using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPredictableDashDownIndicator : MonoBehaviour
{
    [SerializeField] private float _rayRange = float.PositiveInfinity;
    [SerializeField] private LayerMask _layer;
    [SerializeField] private GroundChecker _groundChecker;
    [SerializeField] private Transform _indicatorTransform;
    [SerializeField] private Vector3 _offset;
    private RaycastHit _hit;

    private void Update()
    {
        IndicatePredictably();
    }

    public void IndicatePredictably()
    {
        if (Physics.Raycast(transform.position, -transform.up, out _hit, _rayRange, _layer))
        {
            if (_hit.collider.TryGetComponent<Ground>(out Ground ground) && !_groundChecker.Detected)
            {
                _indicatorTransform.transform.position = _hit.point + _offset;
                return;
            }
        }

        _indicatorTransform.transform.position = new Vector3(-999, -999, -666);
    }
}