using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyChangeNavMeshSpeed : MonoBehaviour
{
    [SerializeField] private float _navMeshSpeed;
    private NavMeshAgent _navMeshAgent;
    private float _defaultNavMeshSpeed;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _defaultNavMeshSpeed = _navMeshAgent.speed;
    }

    private void OnEnable()
    {
        KunitanaUltimate.Ultimated += OnUltimated;
        KunitanaUltimate.UltimateStoped += OnUltimateStoped;
    }

    private void OnUltimateStoped()
    {
        _navMeshAgent.speed = _defaultNavMeshSpeed;
    }

    private void OnUltimated()
    {
        _navMeshAgent.speed = _navMeshSpeed;
    }

    private void OnDisable()
    {
        KunitanaUltimate.Ultimated -= OnUltimated;
        KunitanaUltimate.UltimateStoped -= OnUltimateStoped;
    }
}
