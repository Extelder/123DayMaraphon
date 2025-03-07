using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SelfHarmSound : MonoBehaviour
{
    [SerializeField] private KunitanaHarakiriState _harakiri;

    [Inject] private Pools _pools;

    private void OnEnable()
    {
        _harakiri.HarakiriPerformed += OnHarakiriPerformed;
    }

    private void OnDisable()
    {
        _harakiri.HarakiriPerformed -= OnHarakiriPerformed;
    }

    private void OnHarakiriPerformed()
    {
        _pools.SelfHarmSoundPool.GetFreeElement(transform.position);
    }
}
