using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastBehaviour : MonoBehaviour
{
    [field: SerializeField] public Transform Camera { get; private set; }
    [field: SerializeField] public LayerMask ToIgnore { get; private set; }
    [field: SerializeField] public float Range { get; private set; }

    public bool CheckColliderHasComponent<T>(out Collider collider, out T component) where T : MonoBehaviour
    {
        component = null;
        bool detected = GetHitCollider(out collider) && collider.TryGetComponent<T>(out component);
        return detected;
    }

    public bool CheckColliderHasComponentWithStartAndDirection<T>(out Collider collider, out T component, Vector3 start,
        Vector3 direction) where T : MonoBehaviour
    {
        component = null;
        bool detected = GetHitColliderWithStartAndDirection(out collider, start, direction) &&
                        collider.TryGetComponent<T>(out component);
        return detected;
    }

    public bool GetHitCollider(out Collider collider)
    {
        collider = GetRaycastHit().collider;
        return collider;
    }

    public bool GetHitColliderWithOffset(out Collider collider, Vector3 offset)
    {
        collider = GetRaycastHitWithOffset(offset).collider;
        return collider;
    }

    public bool GetHitColliderWithOffset(out Collider collider, Vector3 offset, out RaycastHit hit)
    {
        hit = GetRaycastHitWithOffset(offset);
        collider = hit.collider;
        
        return collider;
    }

    public bool GetHitColliderWithStartAndDirection(out Collider collider, Vector3 start, Vector3 direction)
    {
        collider = GetRaycastHitWithStartAndDirection(start, direction).collider;
        return collider;
    }

    public RaycastHit GetRaycastHit()
    {
        RaycastHit hit = new RaycastHit();
        ThrowRaycast(ref hit);
        return hit;
    }

    public RaycastHit GetRaycastHitWithOffset(Vector3 offset)
    {
        RaycastHit hit = new RaycastHit();
        ThrowRaycastWithOffset(ref hit, offset);
        return hit;
    }

    public RaycastHit GetRaycastHitWithStartAndDirection(Vector3 start, Vector3 direction)
    {
        RaycastHit hit = new RaycastHit();
        ThrowRaycastWithStartAndDirection(ref hit, start, direction);
        return hit;
    }

    public RaycastHit GetRaycastHitWithOffsetAndStartEndPoints(Vector3 offset, Vector3 startPoint,
        Vector3 direction, Quaternion rotation)
    {
        RaycastHit hit = new RaycastHit();
        ThrowRaycastWithOffsetAndStartEndPoints(ref hit, offset, startPoint, direction, rotation);
        return hit;
    }

    public void ThrowRaycast(ref RaycastHit outHit)
    {
        Physics.Raycast(Camera.transform.position, Camera.forward, out outHit, Range, ~ToIgnore);
    }

    public void ThrowRaycastWithOffset(ref RaycastHit outHit, Vector3 offset)
    {
        Physics.Raycast(Camera.transform.position, Camera.forward + Camera.rotation * offset, out outHit, Range,
            ~ToIgnore);
    }

    public void ThrowRaycastWithStartAndDirection(ref RaycastHit outHit, Vector3 start, Vector3 direction)
    {
        Physics.Raycast(start, direction, out outHit, Range,
            ~ToIgnore);
    }

    public void ThrowRaycastWithOffsetAndStartEndPoints(ref RaycastHit outHit, Vector3 offset, Vector3 startPoint,
        Vector3 direction, Quaternion rotation)
    {
        Physics.Raycast(startPoint, direction + rotation * offset, out outHit, Range,
            ~ToIgnore);
    }
}