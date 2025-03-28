using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceHitBox : MonoBehaviour, IWeaponVisitor
{
    [SerializeField] private ParticleSystem _iceExplodeEffect;
    [SerializeField] private GameObject _iceGFX;
    [SerializeField] private GameObject _iceBrokentGFX;
    [SerializeField] private Collider _collider;

    [SerializeField] private AudioSource _brokenIceSound;
    [SerializeField] private bool _disableCollider;

    public event Action Hitted;

    public void Visit(WeaponShoot weaponShoot)
    {
    }

    public void Visit(KunitanShoot kunitanShoot)
    {
        Hitted?.Invoke();
        if (_disableCollider)
            _collider.enabled = false;
        _iceExplodeEffect.Play();
        _iceGFX.SetActive(false);
        _iceBrokentGFX?.SetActive(true);
        _brokenIceSound.Play();
    }

    public void Visit(RaycastWeaponShoot raycastWeaponShoot, RaycastHit hit)
    {
    }

    public void Visit(Projectile projectile)
    {
    }

    public void Visit(Ghost ghost, float damage)
    {
    }
}