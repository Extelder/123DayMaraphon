using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class AirGunShootState : WeaponShootState
{
    [SerializeField] private float _cdTimeInSeconds;
    [SerializeField] private Rigidbody _playerRigidbody;
    [SerializeField] private float _force;
    [SerializeField] private Transform _camera;

    private CompositeDisposable _disposable = new CompositeDisposable();

    public override void Enter()
    {
        CanChanged = false;
        CanShoot = false;
        Animator.Shoot();

        PerformShoot();

        CoolDown.Timer(_cdTimeInSeconds, () => { CanShoot = true; }, _disposable);
    }


    public void PerformShoot()
    {
        _playerRigidbody.velocity = new Vector3(0, 0, 0);
        _playerRigidbody.AddForce(-_camera.forward * _force, ForceMode.Impulse);
    }

    public void AnimationEndCanChanged()
    {
        CanChanged = true;
    }

    private void OnDisable()
    {
        AnimationEndCanChanged();
        CanShoot = true;
        _disposable.Clear();
    }
}