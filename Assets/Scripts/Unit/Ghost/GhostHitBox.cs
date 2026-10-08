using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Комбо призрака (абилка дробовика):
//   HitScan  — x2 урона по застаненным, если в стане никого — монета;
//   Buckshot — урон по застаненным, если в стане никого — каждая дробина монетой в разных врагов;
//   Мультишот (берст рейлгана) — x2 урона по застаненным, иначе две монеты;
//   Шар      — радиус стана больше + время жизни заново;
//   Ракета   — взрывы призрака по кд, если в стане никого — ракета-монета во врага;
//   Волна    — призрак делится на две половины, любой удар по одной повторяется на второй.
// Катана, как и раньше, просто бьёт по застаненным.
public class GhostHitBox : MonoBehaviour, IWeaponVisitor
{
    private const string CoinPointName = "CoinSpawnPoint";
    private const string DetectSphereName = "DetectSphere";

    [SerializeField] private Animator _animator;
    [SerializeField] private string _rpgShootedTriggetName = "RpgShoot";
    [SerializeField] private string _lightningShootedTriggetName = "LightningShoot";

    [SerializeField] private Ghost _ghost;
    [SerializeField] private Pools _pools;

    [Header("HitScan / Buckshot / Multishot")]
    [SerializeField] private float _hitScanStunDamageMultiplier = 2f;
    [SerializeField] private float _multishotStunDamageMultiplier = 2f;
    [SerializeField] private int _multishotCoins = 2;
    [SerializeField] private float _shotReactionCooldown = 0.25f;

    [Header("Coin")]
    [SerializeField] private float _coinDamageMultiplier = 2f;
    [SerializeField] private float _coinRange = 40f;

    [Header("Ball")]
    [SerializeField] private float _ballRadiusMultiplier = 1.5f;
    [SerializeField] private float _maxRadiusMultiplier = 3f;
    [SerializeField] private float _ballScaleMultiplier = 1.15f;
    [SerializeField] private float _maxBallScale = 1.6f;

    [Header("Rocket")]
    [SerializeField] private float _rocketExplosionCooldown = 0.8f;
    [SerializeField] private float _rocketExplosionDamageMultiplier = 1f;

    [Header("Wave split")]
    [Tooltip("На сколько радиусов зоны каждая половина отходит от исходной точки (1 — круги касаются)")]
    [SerializeField] private float _splitDistanceFactor = 1f;

    public event Action RailGunHitted;
    public event Action RPGProjectilHitted;

    public static event Action RailUniqueHit;
    public static event Action RpgUniqueHit;

    private static readonly Collider[] TargetBuffer = new Collider[64];

    private readonly List<(Collider collider, IWeaponVisitor visitor)> _targets =
        new List<(Collider collider, IWeaponVisitor visitor)>();
    private readonly List<(Collider collider, IWeaponVisitor visitor)> _buckshotTargets =
        new List<(Collider collider, IWeaponVisitor visitor)>();
    private readonly HashSet<object> _handledSources = new HashSet<object>();
    private readonly List<Collider> _ignoredColliders = new List<Collider>();

    private Vector3 _defaultScale;
    private PoolObject _poolObject;
    private Rigidbody _rigidbody;
    private Collider[] _ownColliders;
    private Transform _coinPoint;
    private GhostHitBox _twin;
    private Coroutine _rocketExplosions;
    private float _rocketDamage;
    private int _handledFrame = -1;
    private int _buckshotFrame = -1;
    private int _buckshotIndex;
    private float _lastShotReactionTime = -1f;

    private Transform CoinPoint
    {
        get
        {
            if (_coinPoint != null)
                return _coinPoint;

            // Точку можно задать в префабе дочерним объектом CoinSpawnPoint на руке.
            _coinPoint = FindDeep(transform, CoinPointName);
            if (_coinPoint == null)
            {
                _coinPoint = new GameObject(CoinPointName).transform;
                _coinPoint.SetParent(transform, false);
                _coinPoint.position = EstimateHandPosition();
            }

            return _coinPoint;
        }
    }

    private void Awake()
    {
        _defaultScale = transform.localScale;
        _poolObject = GetComponent<PoolObject>();
        _rigidbody = GetComponent<Rigidbody>();
        _ownColliders = GetComponentsInChildren<Collider>(true);
    }

    private void OnDisable()
    {
        _animator.ResetTrigger(_rpgShootedTriggetName);
        _animator.ResetTrigger(_lightningShootedTriggetName);
        transform.localScale = _defaultScale;
        _rocketExplosions = null;
        _rocketDamage = 0f;
        RestoreIgnoredCollisions();
        Unlink();
    }

    #region Visits

    public void Visit(WeaponShoot weaponShoot)
    {
    }

    public void Visit(KunitanShoot kunitanShoot)
    {
        Handle(null, ghost => ghost.DefaultHit(kunitanShoot.Damage));
    }

    public void Visit(KunitanaUltimateAttack kunitanShoot)
    {
        Handle(null, ghost => ghost.DefaultHit(kunitanShoot.Damage));
    }

    public void Visit(RaycastWeaponShoot raycastWeaponShoot, RaycastHit hit)
    {
        RaycastShotKind kind = raycastWeaponShoot.Kind;
        float damage = raycastWeaponShoot.Damage;
        SpawnHitVfx(hit.point);
        Handle(null, ghost => ghost.OnShot(kind, damage));
        RailUniqueHit?.Invoke();
    }

    public void Visit(Projectile projectile)
    {
        if (projectile.ComboSpawned)
            return;

        if (projectile is LightningBall)
        {
            Handle(projectile, ghost => ghost.OnBall());
        }
        else
        {
            float damage = projectile.Damage;
            Handle(projectile, ghost => ghost.OnRocket(damage));
        }

        RpgUniqueHit?.Invoke();
    }

    public void Visit(Ghost ghost, float damage)
    {
    }

    public void Visit(PlayerSlashProjectile slashProjectile)
    {
        bool wasLinked = _twin != null;
        if (!Handle(slashProjectile, ghost => ghost.DamageTrapedUnits(slashProjectile.Damage)))
            return;

        if (!wasLinked)
            Split(slashProjectile.transform.forward);
    }

    #endregion

    #region Combos

    private void OnShot(RaycastShotKind kind, float damage)
    {
        bool hasStunned = HasTrappedUnits();
        switch (kind)
        {
            case RaycastShotKind.Buckshot:
                if (hasStunned)
                    DamageTrapedUnits(damage);
                else
                    LaunchBuckshotCoin(damage * _coinDamageMultiplier);
                break;

            case RaycastShotKind.Multishot:
                if (hasStunned)
                    DamageTrapedUnits(damage * _multishotStunDamageMultiplier);
                else
                    LaunchCoins(_multishotCoins, damage * _coinDamageMultiplier);
                break;

            default:
                if (hasStunned)
                    DamageTrapedUnits(damage * _hitScanStunDamageMultiplier);
                else
                    LaunchCoins(1, damage * _coinDamageMultiplier);
                break;
        }

        // Реакция призрака (вспышка) не чаще кулдауна — рифл и дробь стреляют часто. Анимацию RpgShoot
        // не запускаем: это анимация расширения зоны, а от выстрелов радиус больше не растёт.
        if (Time.time - _lastShotReactionTime > _shotReactionCooldown)
        {
            _lastShotReactionTime = Time.time;
            RailGunHitted?.Invoke();
        }
    }

    private void OnBall()
    {
        _ghost.ExtendRadius(_ballRadiusMultiplier, _maxRadiusMultiplier);
        transform.localScale = Vector3.Min(transform.localScale * _ballScaleMultiplier, _defaultScale * _maxBallScale);
        if (_poolObject != null)
            _poolObject.RestartLifetime();

        _animator.SetTrigger(_lightningShootedTriggetName);
        SpawnHitVfx(transform.position);
    }

    private void OnRocket(float damage)
    {
        RPGProjectilHitted?.Invoke();

        if (!HasTrappedUnits())
        {
            LaunchRocketCoin();
            return;
        }

        _rocketDamage = Mathf.Max(_rocketDamage, damage * _rocketExplosionDamageMultiplier);
        if (_rocketExplosions == null)
            _rocketExplosions = StartCoroutine(RocketExplosions());
        else
            ExplodeOnce();
    }

    private IEnumerator RocketExplosions()
    {
        while (true)
        {
            ExplodeOnce();
            yield return new WaitForSeconds(_rocketExplosionCooldown);
        }
    }

    private void ExplodeOnce()
    {
        DamageTrapedUnits(_rocketDamage);
        SpawnHitVfx(transform.position);
        ExplosionFx.Play(transform.position, 4f * _ghost.GhostRadiusMultiplier, ExplosionFx.Kind.Ghost);
        _pools.ProjectileSoundPool.GetFreeElement(transform.position, Quaternion.identity);
    }

    private void LaunchRocketCoin()
    {
        Vector3 origin = CoinPoint.position;
        CollectTargets(origin, _targets);

        // Монетка-"щелчок" для читаемости, сама ракета летит во врага.
        ComboCoin.Launch(origin, null, null, 0f, _ghost, _pools);
        if (_targets.Count == 0)
            return;

        Projectile rocket = _pools.DefaultProjectilePool.GetFreeElement(origin, Quaternion.identity)
            .GetComponent<Projectile>();
        if (rocket == null)
            return;

        rocket.ComboSpawned = true;
        IgnoreCollisions(rocket);
        rocket.Initiate(_targets[0].collider.bounds.center);
    }

    private void Split(Vector3 slashDirection)
    {
        Vector3 side = Vector3.Cross(Vector3.up, slashDirection);
        if (side.sqrMagnitude < 0.01f)
            side = transform.right;
        side.Normalize();

        Vector3 center = transform.position;
        GhostHitBox twin;
        try
        {
            twin = _pools.GhostPool.GetFreeElement(center, transform.rotation).GetComponent<GhostHitBox>();
        }
        catch (Exception exception)
        {
            Debug.LogWarning("Ghost split skipped: " + exception.Message);
            return;
        }

        if (twin == null || twin == this)
            return;

        // Половины того же размера, что и призрак (без расширения), и расходятся в стороны так,
        // чтобы их зоны не пересекались.
        twin.transform.localScale = transform.localScale;
        twin._ghost.GhostRadiusMultiplier = _ghost.GhostRadiusMultiplier;

        float distance = CircleRadius() * _splitDistanceFactor;
        MoveTo(center - side * distance);
        twin.MoveTo(center + side * distance);

        _twin = twin;
        twin._twin = this;

        if (_poolObject != null)
            _poolObject.RestartLifetime();

        SpawnHitVfx(transform.position);
        twin.SpawnHitVfx(twin.transform.position);
    }

    private void DefaultHit(float damage)
    {
        DamageTrapedUnits(damage);
        SpawnHitVfx(transform.position);
    }

    #endregion

    #region Coins

    private void LaunchCoins(int count, float damage)
    {
        Vector3 origin = CoinPoint.position;
        CollectTargets(origin, _targets);

        for (int i = 0; i < count; i++)
        {
            if (_targets.Count == 0)
            {
                ComboCoin.Launch(origin, null, null, damage, _ghost, _pools);
                continue;
            }

            (Collider collider, IWeaponVisitor visitor) target = _targets[i % _targets.Count];
            ComboCoin.Launch(origin, target.collider, target.visitor, damage, _ghost, _pools);
        }
    }

    // Дробины одного выстрела приходят в одном кадре — раздаём их по разным врагам по кругу.
    private void LaunchBuckshotCoin(float damage)
    {
        Vector3 origin = CoinPoint.position;
        if (_buckshotFrame != Time.frameCount)
        {
            _buckshotFrame = Time.frameCount;
            _buckshotIndex = 0;
            CollectTargets(origin, _buckshotTargets);
        }

        if (_buckshotTargets.Count == 0)
        {
            ComboCoin.Launch(origin, null, null, damage, _ghost, _pools);
            return;
        }

        (Collider collider, IWeaponVisitor visitor) target = _buckshotTargets[_buckshotIndex % _buckshotTargets.Count];
        _buckshotIndex++;
        ComboCoin.Launch(origin, target.collider, target.visitor, damage, _ghost, _pools);
    }

    private void CollectTargets(Vector3 origin, List<(Collider collider, IWeaponVisitor visitor)> targets)
    {
        targets.Clear();
        int count = Physics.OverlapSphereNonAlloc(origin, _coinRange, TargetBuffer, _ghost.TargetLayer,
            QueryTriggerInteraction.Collide);

        for (int i = 0; i < count; i++)
        {
            Collider collider = TargetBuffer[i];
            IWeaponVisitor visitor = null;
            if (collider.TryGetComponent(out IGhostTrapable trapable))
                visitor = trapable.ObjectVisitor;
            else if (collider.TryGetComponent(out UnitHitBox hitBox))
                visitor = hitBox;

            if (visitor == null || !ComboCoin.IsAlive(visitor) || targets.Any(target => target.visitor == visitor))
                continue;

            targets.Add((collider, visitor));
        }

        targets.Sort((a, b) => (a.collider.bounds.center - origin).sqrMagnitude
            .CompareTo((b.collider.bounds.center - origin).sqrMagnitude));
    }

    #endregion

    #region Helpers

    // Применяет комбо к себе и к второй половине. source защищает от двойной обработки,
    // когда один взрыв или волна задевает обе половины.
    private bool Handle(object source, Action<GhostHitBox> combo)
    {
        if (source != null && !MarkHandled(source))
            return false;

        combo(this);

        if (_twin != null && _twin.isActiveAndEnabled && (source == null || _twin.MarkHandled(source)))
            combo(_twin);

        return true;
    }

    private bool MarkHandled(object source)
    {
        if (_handledFrame != Time.frameCount)
        {
            _handledFrame = Time.frameCount;
            _handledSources.Clear();
        }

        return _handledSources.Add(source);
    }

    private bool HasTrappedUnits()
    {
        foreach (IGhostTrapable traped in _ghost.TrapedUnits)
        {
            if (traped is UnityEngine.Object unityObject && unityObject != null
                && traped.ObjectVisitor != null && ComboCoin.IsAlive(traped.ObjectVisitor))
                return true;
        }

        return false;
    }

    private void DamageTrapedUnits(float damage)
    {
        foreach (var traped in _ghost.TrapedUnits.ToList())
        {
            if (traped.ObjectVisitor != null)
                traped.ObjectVisitor.Visit(_ghost, damage);
        }
    }

    private void SpawnHitVfx(Vector3 position)
    {
        _pools.GhostBloodExplodePool.GetFreeElement(position);
    }

    private void IgnoreCollisions(Projectile rocket)
    {
        Collider rocketCollider = rocket.GetComponent<Collider>();
        if (rocketCollider == null)
            return;

        SetIgnored(rocketCollider, true);
        _ignoredColliders.Add(rocketCollider);

        Action restore = null;
        restore = () =>
        {
            rocket.Exploded -= restore;
            if (_ignoredColliders.Remove(rocketCollider))
                SetIgnored(rocketCollider, false);
        };
        rocket.Exploded += restore;
    }

    private void RestoreIgnoredCollisions()
    {
        foreach (Collider rocketCollider in _ignoredColliders)
        {
            if (rocketCollider != null)
                SetIgnored(rocketCollider, false);
        }

        _ignoredColliders.Clear();
    }

    private void SetIgnored(Collider other, bool ignore)
    {
        foreach (Collider own in _ownColliders)
        {
            if (own != null)
                Physics.IgnoreCollision(other, own, ignore);
        }
    }

    // Радиус "круга" призрака для разлёта половин: наибольший из зоны стана, видимой сферы и её обводки
    // (по мешу, независимо от того, включена ли сфера) — круги встают ровно на стык и не пересекаются.
    private float CircleRadius()
    {
        float radius = _ghost.StunRadius;
        foreach (MeshFilter filter in GetComponentsInChildren<MeshFilter>(true))
        {
            if (!filter.name.StartsWith(DetectSphereName) || filter.sharedMesh == null
                || HasExplosionAncestor(filter.transform))
                continue;

            Vector3 scale = filter.transform.lossyScale;
            Vector3 extents = filter.sharedMesh.bounds.extents;
            float sphereRadius = Mathf.Max(extents.x * Mathf.Abs(scale.x), extents.z * Mathf.Abs(scale.z));
            radius = Mathf.Max(radius, sphereRadius);
        }

        return radius;
    }

    private bool HasExplosionAncestor(Transform target)
    {
        for (Transform current = target.parent; current != null && current != transform; current = current.parent)
        {
            if (current.name.Contains("Explosion"))
                return true;
        }

        return false;
    }

    private void MoveTo(Vector3 position)
    {
        if (_rigidbody != null)
            _rigidbody.position = position;
        transform.position = position;
    }

    private void Unlink()
    {
        if (_twin != null && _twin._twin == this)
            _twin._twin = null;
        _twin = null;
    }

    // У модели призрака нет скелета: берём боковую часть самого крупного меша как "руку".
    private Vector3 EstimateHandPosition()
    {
        Renderer body = null;
        float bodySize = 0f;
        foreach (Renderer candidate in GetComponentsInChildren<Renderer>())
        {
            if (!(candidate is MeshRenderer) && !(candidate is SkinnedMeshRenderer))
                continue;

            float size = candidate.bounds.size.sqrMagnitude;
            if (size > bodySize)
            {
                body = candidate;
                bodySize = size;
            }
        }

        if (body == null)
            return transform.position + Vector3.up;

        Bounds bounds = body.bounds;
        return bounds.center + transform.right * (bounds.extents.x * 0.75f) + Vector3.up * (bounds.extents.y * 0.2f);
    }

    private static Transform FindDeep(Transform root, string name)
    {
        foreach (Transform child in root)
        {
            if (child.name == name)
                return child;

            Transform found = FindDeep(child, name);
            if (found != null)
                return found;
        }

        return null;
    }

    #endregion
}
