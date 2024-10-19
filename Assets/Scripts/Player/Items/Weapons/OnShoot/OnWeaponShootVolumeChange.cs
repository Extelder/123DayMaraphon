using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering;

public class OnWeaponShootVolumeChange : MonoBehaviour
{
    [SerializeField] private float _lerpVolumeWeightSpeed;
    [SerializeField] private float _backToDefaultVolumeTime;
    [SerializeField] private Volume _volume;
    [SerializeField] private WeaponShoot _weaponShoot;

    private float _targetVolumeWeight;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private void OnEnable()
    {
        _weaponShoot.ShootPerformed += OnShootPerformed;
    }

    private void OnDisable()
    {
        _weaponShoot.ShootPerformed -= OnShootPerformed;
        _volume.weight = 0;
        _targetVolumeWeight = 0;
        _disposable.Clear();
    }

    private void OnShootPerformed()
    {
        _disposable.Clear();
        _targetVolumeWeight = 1;
        CoolDown.Timer(_backToDefaultVolumeTime, () => { _targetVolumeWeight = 0; }, _disposable);
    }

    private void Update()
    {
        _volume.weight = Mathf.Lerp(_volume.weight, _targetVolumeWeight, _lerpVolumeWeightSpeed * Time.deltaTime);
    }
}