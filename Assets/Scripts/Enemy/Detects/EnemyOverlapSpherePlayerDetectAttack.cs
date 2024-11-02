using System;
using System.Collections;
using UnityEngine;


public class EnemyOverlapSpherePlayerDetectAttack : MonoBehaviour
{
    [SerializeField] private Transform _checkPoint;
    [SerializeField] private float _checkRange;
    [SerializeField] private LayerMask _checkMask;
    [SerializeField] private EnemyStateMachine _enemyStateMachine;
    [SerializeField] private float _chekingRate = 0.2f;

    private void Awake()
    {
        StartCoroutine(Checking());
    }

    private IEnumerator Checking()
    {
        while (true)
        {
            yield return new WaitForSeconds(_chekingRate);
            Collider[] colliders = new Collider[1];
            Physics.OverlapSphereNonAlloc(_checkPoint.position, _checkRange, colliders, _checkMask);
            if (colliders[0] != null)
            {
                colliders[0] = null;
                _enemyStateMachine.Attack();
            }
            else
            {
                _enemyStateMachine.Move();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_checkPoint.position, _checkRange);
    }
}