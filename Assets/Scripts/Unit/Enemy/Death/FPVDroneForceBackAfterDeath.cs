using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class FPVDroneForceBackAfterDeath : MonoBehaviour
{
    [Inject] private Pools _pools;
    
    [SerializeField] private FlyEnemyHealth _health;
    [SerializeField] private float _forceBackStrength;
    [SerializeField] private float _forceDowmStrength;
    [SerializeField] private EnemyGroundChecker _groundChecker;
    [SerializeField] private ParticleSystem _explosionParticle;
    [SerializeField] private GameObject _parent;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _health.Dead += StartForceBack;
    }

    private void StartForceBack()
    {
        _rigidbody.useGravity = true;
        StartCoroutine(ForceBack());
    }

    private IEnumerator ForceBack()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.02f);
            _rigidbody.AddForce(-transform.forward * _forceBackStrength, ForceMode.Impulse);
            _rigidbody.AddForce(-transform.up * _forceDowmStrength, ForceMode.Impulse);
            if (_groundChecker.Detected)
            {
                _explosionParticle.transform.parent = null;
                _explosionParticle.Play();
                _pools.ProjectileSoundPool.GetFreeElement(transform.position, Quaternion.identity);
                _rigidbody.useGravity = false;
                Destroy(_parent);
            }
        }
    }

    private void OnDisable()
    {
        _health.Dead -= StartForceBack;
    }
}