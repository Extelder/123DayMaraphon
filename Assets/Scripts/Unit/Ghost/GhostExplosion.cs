using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostExplosion : MonoBehaviour
{
    [SerializeField] private GhostHitBox _ghostHitBox;

    [SerializeField] private GameObject _defaultExplosion;
    [SerializeField] private GameObject _coolExplosion;


    private void OnEnable()
    {
        _ghostHitBox.RailGunHitted += OnRailGunHitted;
        _ghostHitBox.RPGProjectilHitted += OnRPGProjectileHitted;
    }

    private void OnRPGProjectileHitted()
    {
        _defaultExplosion.SetActive(true);
    }

    private void OnRailGunHitted()
    {
        _defaultExplosion.SetActive(false);
        _coolExplosion.SetActive(true);
    }

    private void OnDisable()
    {
        _ghostHitBox.RailGunHitted -= OnRailGunHitted;
        _ghostHitBox.RPGProjectilHitted -= OnRPGProjectileHitted;
        _defaultExplosion.SetActive(true);
        _coolExplosion.SetActive(false);
    }
}