using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingStateMachine : EnemyStateMachine
{
    private void Start()
    {
        StartCoroutine(Moving());
    }

    private IEnumerator Moving()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            Debug.Log("MMoving");
            Move();
        }
    }
}