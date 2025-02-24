using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Zenject;

public class Spikes : MonoBehaviour
{
    [Inject] private PlayerHealth _playerHealth;

    [SerializeField] private float _damage;
    [SerializeField] private float _cooldown;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<PlayerHitBox>(out PlayerHitBox _player))
        {
            StartCoroutine(TakeDamage());
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.TryGetComponent<PlayerHitBox>(out PlayerHitBox _player))
        {
            StopAllCoroutines();
        }
    }

    private IEnumerator TakeDamage()
    {
        while (true)
        {
            _playerHealth.TakeDamage(_damage);
            yield return new WaitForSeconds(_cooldown);
        }
    }
}
