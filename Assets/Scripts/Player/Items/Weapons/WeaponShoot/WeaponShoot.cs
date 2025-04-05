using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponShoot : RaycastBehaviour, IHypeMeasurable
{
    [field: SerializeField] public float HypeValue { get; set; }= 0.1f;
    [field: SerializeField] public HypeType HypeType { get; set; }


    [SerializeField] private DefaultWeaponShootState _weaponShootState;

    [field: SerializeField] public RaycastWeaponItem Weapon { get; private set; }

    public Vector3 CurrentShootOffset;

    public event Action ShootPerformed;
    public event Action CameraShake;

    private void OnEnable()
    {
        _weaponShootState.ShootPerformed += OnShootPerformed;
    }

    private void OnDisable()
    {
        _weaponShootState.ShootPerformed -= OnShootPerformed;
    }

    public virtual void OnShootPerformed()
    {
        ShootPerformed?.Invoke();
    }

    public virtual void Accept(IWeaponVisitor visitor)
    {
        visitor.Visit(this);
    }

    public void CameraShakeInvoke()
    {
        CameraShake?.Invoke();
    }
}