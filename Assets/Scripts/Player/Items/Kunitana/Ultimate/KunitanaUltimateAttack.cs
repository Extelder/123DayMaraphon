using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
public class KunitanaUltimateAttack : MonoBehaviour, IHypeMeasurable
{
    [Inject] private PlayerInputs _inputs;
    [field: SerializeField] public float Damage { get; private set; }

    [SerializeField] private OverlapSettings _overlapSettings;

    private Animator _animator;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _inputs.PlayerWeaponInputs.KunitanaUltimateKeyUp += OnKunitanaShootPressedUp;
        _inputs.PlayerWeaponInputs.KunitanaUltimateKeyDowm += OnKunitanaShootPressedDown;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_overlapSettings._overlapPoint.position, _overlapSettings._sphereRadius);
    }

    private void OverlapSphere()
    {
        _overlapSettings.Colliders = new Collider[10];
        _overlapSettings.Size = Physics.OverlapSphereNonAlloc(
            _overlapSettings._overlapPoint.position + _overlapSettings._positionOffset,
            _overlapSettings._sphereRadius, _overlapSettings.Colliders,
            _overlapSettings._searchLayer);
    }

    public void PerformShoot()
    {
        _animator.SetInteger("States", Random.Range(1, 3));
        OverlapSphere();
        foreach (var other in _overlapSettings.Colliders)
        {
            if (other == null)
                continue;
            if (other.TryGetComponent<IWeaponVisitor>(out IWeaponVisitor weaponVisitor))
            {
                weaponVisitor.Visit(this);
            }
        }
    }

    private void OnKunitanaShootPressedDown()
    {
        _animator.SetInteger("States", Random.Range(1, 3));
        _animator.SetBool("Attack", true);
    }
    
    private void OnKunitanaShootPressedUp()
    {
        _animator.SetInteger("States", 0);
        _animator.SetBool("Attack", false);
    }

    private void OnDisable()
    {
        _inputs.PlayerWeaponInputs.KunitanaShootPressedUp -= OnKunitanaShootPressedUp;
        _inputs.PlayerWeaponInputs.KunitanaShootPressedDown -= OnKunitanaShootPressedDown;
    }

    public float HypeValue { get; set; }
}
