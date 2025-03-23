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

    public event Action Hitted;
    
    public void Visit(WeaponShoot weaponShoot)
    {
    }

    public void Visit(KunitanShoot kunitanShoot)
    {
        Hitted?.Invoke();
        _collider.enabled = false;
        _iceExplodeEffect.Play();
        _iceGFX.SetActive(false);
        _iceBrokentGFX?.SetActive(true);
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