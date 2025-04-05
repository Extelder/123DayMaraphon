using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunitanShoot : MonoBehaviour, IHypeMeasurable
{
    [field: SerializeField] public float HypeValue { get; set; } = 0.1f;
    [field: SerializeField] public HypeType HypeType { get; set; }
    [field: SerializeField] public float HitHypeMultiplier { get; set; } = 1.1f;

    [SerializeField] private DefaultWeaponShootState _weaponShootState;
    [SerializeField] private OverlapSettings _overlapSettings;
    [SerializeField] private float _kayotTime = 0.1f;
    [SerializeField] private float _stopTime = 0.2f;

    [field: SerializeField] public float Damage { get; private set; }

    public static KunitanShoot Instance { get; private set; }

    private RaycastWeaponShoot _lastShoot;

    private Coroutine _coroutine;

    public event Action Shooted;
    public event Action TimeStopped;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_overlapSettings._overlapPoint.position, _overlapSettings._sphereRadius);
    }

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

    public void SetLastShoot(RaycastWeaponShoot weaponShoot)
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

    private void OverlapSphere()
    {
        _overlapSettings.Colliders = new Collider[10];
        _overlapSettings.Size = Physics.OverlapSphereNonAlloc(
            _overlapSettings._overlapPoint.position + _overlapSettings._positionOffset,
            _overlapSettings._sphereRadius, _overlapSettings.Colliders,
            _overlapSettings._searchLayer);
    }

    public virtual void OnShootPerformed()
    {
        Shooted?.Invoke();

        if (_lastShoot != null)
        {
            _lastShoot.OnKunitanShootPerformed();
            PlayerTime.Instance.TimeStop(_stopTime);
            TimeStopped?.Invoke();
        }

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

}