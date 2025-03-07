using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLastOutline : MonoBehaviour
{
    [SerializeField] private Outline _outline;


    private int _defaultLayerMask;

    private void Start()
    {
        _defaultLayerMask = _outline.gameObject.layer;
        StartCoroutine(Checking());
    }

    private void OnDisable()
    {
        DisableOutline();
    }

    private IEnumerator Checking()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            if (EnemyWaveSystem.CurrentWaveSystem == null)
                continue;

            yield return new WaitUntil(() =>
                EnemyWaveSystem.CurrentWaveSystem.HowMuchEnemyLost() <= EnemyWaveSystem.CurrentWaveSystem.EnemyForOutline);
            EnableOutline();
        }
    }

    public void EnableOutline()
    {
        _outline.enabled = true;
        _outline.gameObject.layer = LayerMask.NameToLayer("Overlay");
    }

    public void DisableOutline()
    {
        _outline.enabled = false;
        _outline.gameObject.layer = _defaultLayerMask;
    }
}