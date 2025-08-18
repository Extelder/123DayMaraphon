using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

public class PlayerSlashProjectile : PoolObject, ISlashProjectile, IWeaponVisitor, IHypeMeasurable
{
    [field: SerializeField] public float Damage { get; set; }
    [field: SerializeField] public float Speed { get; set; }
    [field: SerializeField] public float CharacterUpForce { get; set; }
    [field: SerializeField] public Collider Collider { get; set; }
    [field: SerializeField] public float HypeValue { get; set; }
    [field: SerializeField] public HypeType HypeType { get; set; }

    
    private PlayerCharacter _playerCharacter;

    public event Action Triggered;

    private Rigidbody _rigidbody;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private void Awake()
    {
        _playerCharacter = PlayerCharacter.Instance;
        _rigidbody = GetComponent<Rigidbody>();
    }


    public void Initiate()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);

        _rigidbody.velocity = new Vector3(0, 0, 0);
        transform.eulerAngles = _playerCharacter.Camera.eulerAngles;
        _rigidbody.AddForce(transform.forward * Speed, ForceMode.Impulse);
        Collider.OnTriggerEnterAsObservable().Subscribe(other =>
        {
            if (other.TryGetComponent<IWeaponVisitor>(out IWeaponVisitor visitor))
            {
                Debug.Log("watafak");
                visitor.Visit(this, Damage);
                Triggered?.Invoke();
            }
        }).AddTo(_disposable);
    }
    
    private void OnDisable()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
        transform.localScale = new Vector3(0, 0, 0);
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

    public void Visit(PlayerSlashProjectile slashProjectile, float damage)
    {
    }
}
