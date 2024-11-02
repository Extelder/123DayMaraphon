using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggerPlayerDetectMove : MonoBehaviour
{
    [SerializeField] private EnemyStateMachine _enemyStateMachine;

    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerCharacter>(out PlayerCharacter playerCharacter))
        {
            StartCoroutine(Moving());
            Destroy(_collider);
        }
    }

    private IEnumerator Moving()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            Debug.Log("MMoving" );
            _enemyStateMachine.Move();
        }
    }
}