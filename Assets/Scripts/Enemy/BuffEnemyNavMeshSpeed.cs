using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuffEnemyNavMeshSpeed : MonoBehaviour, IBuffable
{
    [SerializeField] private GameObject _buffParticle;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _speedBuffMultiplayer;

    public void Buff()
    {
        _buffParticle.SetActive(true);
        _agent.speed *= _speedBuffMultiplayer;
    }

    public void DeBuff()
    {
        _buffParticle.SetActive(false);
        _agent.speed /= _speedBuffMultiplayer;
    }

    private void OnDisable()
    {
        DeBuff();
    }
}