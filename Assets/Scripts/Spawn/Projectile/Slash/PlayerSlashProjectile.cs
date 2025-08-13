using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class PlayerSlashProjectile : PoolObject, ISlashProjectile, IWeaponVisitor
{
    [field: SerializeField] public float Damage { get; set; }
    [field: SerializeField] public float Speed { get; set; }
    [field: SerializeField] public float CharacterUpForce { get; set; }
    [field: SerializeField] public Collider Collider { get; set; }
    
    [SerializeField] private float _hypeMultiplier;

    public event Action Triggered;

    private Rigidbody _rigidbody;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }


    public void Initiate()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);

        _rigidbody.velocity = new Vector3(0, 0, 0);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        _rigidbody.AddForce(transform.forward * Speed, ForceMode.Impulse);
        Collider.OnTriggerEnterAsObservable().Subscribe(other =>
        {
            if (other.TryGetComponent<UnitHitBox>(out UnitHitBox hitBox))
            {
                hitBox.GetComponent<Rigidbody>()
                    .AddForce(hitBox.transform.up * CharacterUpForce, ForceMode.Impulse);
                hitBox.TakeDamage(Damage, HypeType.AHHHHHHH, _hypeMultiplier);
                Triggered?.Invoke();
            }
        }).AddTo(_disposable);
    }
    
    private void OnDisable()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
        _disposable.Clear();
    }

    public void Visit(WeaponShoot weaponShoot)
    {
    }

    public void Visit(KunitanShoot kunitanShoot)
    {
    }

    public void Visit(KunitanaUltimateAttack kunitanShoot)
    {
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
