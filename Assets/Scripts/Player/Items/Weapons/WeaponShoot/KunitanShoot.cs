using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunitanShoot : MonoBehaviour
{
    [SerializeField] private DefaultWeaponShootState _weaponShootState;
    [SerializeField] private float _kayotTime = 0.1f;
    [SerializeField] private float _stopTime = 0.2f;

    public static KunitanShoot Instance { get; private set; }

    private WeaponShoot _lastShoot;

    private Coroutine _coroutine;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            return;
        }

        Debug.LogError("THERE`S one more kunitana shoot - " + gameObject.name);
        Debug.Break();
    }


    private void OnEnable()
    {
        _weaponShootState.ShootPerformed += OnShootPerformed;
    }

    private void OnDisable()
    {
        _weaponShootState.ShootPerformed -= OnShootPerformed;
    }

    public void SetLastShoot(WeaponShoot weaponShoot)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _lastShoot = weaponShoot;
        _coroutine = StartCoroutine(WaitingKayotTime());
    }

    private IEnumerator WaitingKayotTime()
    {
        yield return new WaitForSeconds(_kayotTime);
        _lastShoot = null;
    }

    public virtual void OnShootPerformed()
    {
        if (_lastShoot != null)
        {
            _lastShoot.OnShootPerformed();
            PlayerTime.Instance.TimeStop(_stopTime);
        }
    }
}