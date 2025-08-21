using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerCharacter : MonoBehaviour
{
    [field: SerializeField] public Transform Transform { get; private set; }
    [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
    [field: SerializeField] public PlayerLevelCheckPoint PlayerLevelCheckPoint { get; private set; }

    [field: SerializeField] public Transform PlaceForTextLerping { get; private set; }
    [field: SerializeField] public Transform Camera { get; private set; }
    [field: SerializeField] public WeaponSwitch WeaponSwitch { get; private set; }
    [field: SerializeField] public PlayerHints Hints { get; private set; }

    public Vector3 PlayerPositionForNavMesh { get; private set; }
    [SerializeField] private float _rayRange;
    [SerializeField] private float _randomNavMeshRadius;
    [SerializeField] private LayerMask _layer;
    [SerializeField] private Vector3 _offset;

    private RaycastHit _hit;
    public static PlayerCharacter Instance { get; private set; }


    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            return;
        }

        Debug.LogError("THERE`s one more PlAYERCHARACTER");
    }


    private void Update()
    {
        if (Physics.Raycast(Transform.position, -Transform.up, out _hit, _rayRange, _layer))
        {
            if (_hit.collider.TryGetComponent<Ground>(out Ground ground))
            {
                PlayerPositionForNavMesh = _hit.point + _offset;
                return;
            }
        }
    }

    public void ResetVelocity()
    {
        Rigidbody.velocity = new Vector3(0, 0, 0);
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }

        return finalPosition;
    }


    public Vector3 RandomNavmeshLocation()
    {
        return RandomNavmeshLocation(_randomNavMeshRadius);
    }
}