using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DisableNavMeshUpdateRotation : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    
    private void Start()
    {
        _agent.updateRotation = false;
    }
}
