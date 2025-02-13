using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectByMouseHitPoint : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _moveObject;
    [SerializeField] private LayerMask _layerMask;

    private void Update()
    {
        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, _layerMask))
        {
            _moveObject.position = hit.point;
        }
    }
}