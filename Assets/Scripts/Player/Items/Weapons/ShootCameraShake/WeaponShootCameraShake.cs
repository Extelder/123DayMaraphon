using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using MilkShake;

public class WeaponShootCameraShake : MonoBehaviour
{
    [SerializeField] private WeaponShoot _weaponShoot;
    [SerializeField] private ShakePreset _shakePreset;
    [SerializeField] private Shaker _shaker;
    [SerializeField] private Transform _camera;

    private void OnEnable()
    {
        _weaponShoot.CameraShake += ShootPerformed;
    }

    private void OnDisable()
    {
        _weaponShoot.CameraShake -= ShootPerformed;
    }

    private void ShootPerformed()
    {
        _shaker.Shake(_shakePreset);
        
     /*   var cameraTransform = _camera.transform;
        _camera.DOShakePosition(0.6f, new Vector3(0.0035f, 0.0035f, 0.0035f) * 2, 10, 0, false, true,
                ShakeRandomnessMode.Full)
            .SetEase(Ease.InOutBounce)
            .SetLink(_camera.transform.gameObject);

        _camera.DOShakeRotation(0.3f, new Vector3(10, 2, 0), 1, 0, true)
            .SetEase(Ease.OutExpo)
            .SetLink(_camera.transform.gameObject); */
    }
}