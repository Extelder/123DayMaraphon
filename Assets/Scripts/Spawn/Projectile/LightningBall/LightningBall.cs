using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBall : Projectile, IWeaponVisitor
{
    [SerializeField] private GameObject _defaultExplosion;
    [SerializeField] private GameObject _hitExplosion;

    [SerializeField] private GameObject _explosion;

    [SerializeField] private float _railGunRangeMultipier;
    [SerializeField] private float _railGunDamageMultipier;

    [Tooltip("Множитель скорости полёта шара (меньше — медленнее, проще попасть для роста)")]
    [SerializeField] private float _flightSpeedMultiplier = 0.4f;

    [Header("Combo: HitScan - grow, then burst")]
    [Tooltip("Рост размера за выстрел: 1.5^4 ≈ x5 за 4 попадания")]
    [SerializeField] private float _growStepScale = 1.5f;
    [SerializeField] private float _growStepRange = 1.22f;
    [SerializeField] private float _growStepDamage = 1.5f;
    [SerializeField] private int _growStepsToBurst = 4;
    [SerializeField] private float _burstDelay = 0.12f;
    [SerializeField] private float _burstDamageMultiplier = 1.5f;
    [SerializeField] private float _burstTimeStop = 0.15f;

    [Header("Combo: Multishot - nuke")]
    [SerializeField] private float _nukeRangeMultiplier = 4f;
    [SerializeField] private float _nukeDamageMultiplier = 6f;
    [SerializeField] private float _nukeTimeStop = 0.45f;
    [Tooltip("Во сколько раз растёт видимая сфера взрыва (урон и радиус — по _nukeRangeMultiplier)")]
    [SerializeField] private float _nukeVisualMultiplier = 2f;

    private float _defaultRange;
    private Vector3 _defaultScale;
    private Vector3 _defaultRootScale;
    private int _growSteps;
    private int _lastGrowFrame = -1;
    private bool _nuking;
    private bool _bursting;
    private EnergyOrb _orb;

    public static event Action Hitted;
    public static event Action<Vector3> Nuked;

    public override void OnDisableVirtual()
    {
        ExplosionRange = _defaultRange;
        _hitExplosion.transform.localScale = _defaultScale;
        transform.localScale = _defaultRootScale;
        _growSteps = 0;
        _nuking = false;
        _bursting = false;
        CancelInvoke(nameof(Burst));
        if (_orb != null)
            _orb.Hide();
        _defaultExplosion.SetActive(true);
        _hitExplosion.SetActive(false);
    }

    private void Start()
    {
        _defaultRange = ExplosionRange;
        _defaultScale = _hitExplosion.transform.localScale;
        _defaultRootScale = transform.localScale;
        Exploded += HideOrb;
    }

    public override void Initiate(Vector3 targetPosition, bool useTargetPosition = true)
    {
        base.Initiate(targetPosition, useTargetPosition);
        SphereCollider sphere = GetComponent<SphereCollider>();
        _orb = EnergyOrb.Attach(transform, sphere != null ? sphere.radius : 2f);
    }

    // В сингулярности шар становится её ядром: энергию раздувает до максимума.
    protected override float LaunchSpeedMultiplier => _flightSpeedMultiplier;

    protected override void OnCaptured()
    {
        CancelInvoke(nameof(Burst));
        if (_orb != null)
        {
            _orb.SetCharge(1f);
            _orb.Flash();
        }
    }

    private void HideOrb()
    {
        if (_orb != null)
            _orb.Hide();
    }

    public void Visit(WeaponShoot weaponShoot)
    {
    }

    public void Visit(KunitanShoot kunitanShoot)
    {
        _defaultExplosion.SetActive(false);
        _hitExplosion.SetActive(true);

        _hitExplosion.transform.localScale *= _railGunRangeMultipier;
        ExplosionRange *= _railGunRangeMultipier;
        Hitted?.Invoke();
        Explode(_railGunDamageMultipier);
        PlayerTime.Instance.TimeStop(0.3f);
    }

    public void Visit(KunitanaUltimateAttack kunitanShoot)
    {
        _defaultExplosion.SetActive(false);
        _hitExplosion.SetActive(true);

        _hitExplosion.transform.localScale *= _railGunRangeMultipier;
        ExplosionRange *= _railGunRangeMultipier;
        Hitted?.Invoke();
        Explode(_railGunDamageMultipier);
        PlayerTime.Instance.TimeStop(0.3f);
    }

    public void Visit(RaycastWeaponShoot raycastWeaponShoot, RaycastHit hit)
    {
        if (raycastWeaponShoot.Kind == RaycastShotKind.Multishot)
            Nuke();
        else
            Grow();
    }

    // Шар + ракета и шар + волна в списке комбо нет — шар их игнорирует.
    public void Visit(Projectile projectile)
    {
    }

    public void Visit(Ghost ghost, float damage)
    {
    }

    public void Visit(PlayerSlashProjectile slashProjectile)
    {
    }

    protected override ExplosionFx.Kind GetExplosionKind(float damageMultiplier)
    {
        if (_nuking)
            return ExplosionFx.Kind.Nuke;
        // Почти полностью раздутый шар, задевший стену, тоже взрывается как "полный".
        bool almostFull = _growSteps >= _growStepsToBurst - 1;
        return _bursting || almostFull ? ExplosionFx.Kind.BallMax : ExplosionFx.Kind.Ball;
    }

    // Шар + HitScan: каждый выстрел (дробь — один раз за выстрел) раздувает шар и его урон.
    // После последнего шага шар (x5) коротко "заряжается" и взрывается сам.
    private void Grow()
    {
        if (_lastGrowFrame == Time.frameCount || _growSteps >= _growStepsToBurst || HasExploded)
            return;

        _lastGrowFrame = Time.frameCount;
        _growSteps++;
        transform.localScale *= _growStepScale;
        ExplosionRange *= _growStepRange;
        BonusDamageMultiplier *= _growStepDamage;
        Hitted?.Invoke();

        float charge = (float)_growSteps / _growStepsToBurst;
        if (_orb != null)
        {
            _orb.SetCharge(charge);
            _orb.Flash();
        }
        Color color = Color.Lerp(new Color(0.4f, 0.85f, 1f), LevelPalette.Color, charge);
        ComboFx.FlashLight(transform.position, color, 6f + 10f * charge, 6f + 14f * charge, 0.25f);
        ComboFx.SpawnFx(ComboAssets.Instance != null ? ComboAssets.Pick(ComboAssets.Instance.Sparks) : null,
            transform.position, transform.localScale.x * 0.5f);
        PlayerTime.Instance.TimeStop(0.04f + 0.03f * charge);

        if (_growSteps >= _growStepsToBurst)
            Invoke(nameof(Burst), _burstDelay);
    }

    private void Burst()
    {
        if (HasExploded)
            return;

        _bursting = true;
        _defaultExplosion.SetActive(false);
        _hitExplosion.SetActive(true);
        _hitExplosion.transform.localScale *= _growStepScale;
        Hitted?.Invoke();
        Explode(_burstDamageMultiplier);
        PlayerTime.Instance.TimeStop(_burstTimeStop);
    }

    // Шар + мультишот (берст рейлгана): ядерный взрыв.
    private void Nuke()
    {
        _defaultExplosion.SetActive(false);
        _hitExplosion.SetActive(true);
        _hitExplosion.transform.localScale *= _nukeVisualMultiplier;
        ExplosionRange *= _nukeRangeMultiplier;

        Hitted?.Invoke();
        Nuked?.Invoke(transform.position);
        _nuking = true;
        Explode(_nukeDamageMultiplier);
        PlayerTime.Instance.TimeStop(_nukeTimeStop);
    }
}
