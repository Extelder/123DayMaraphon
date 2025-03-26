using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PUSSlamSound : MonoBehaviour
{
    [SerializeField] private PUSPunchState _pusPunchState;
    [SerializeField] private PUSSlamState _pusSlamState;

    [Inject] private Pools _pools;

    private void OnEnable()
    {
        _pusPunchState.ArmSlamed += OnSlamed;
        _pusSlamState.Slamed += OnSlamed;
    }

    private void OnDisable()
    {
        _pusPunchState.ArmSlamed -= OnSlamed;
        _pusSlamState.Slamed -= OnSlamed;
    }

    private void OnSlamed()
    {
        _pools.PUSSlamSoundPool.GetFreeElement(transform.position);
    }
}
