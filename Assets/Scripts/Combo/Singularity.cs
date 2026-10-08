using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

// Комбо "снаряд (ракета или шар) + волна РПГ" — сингулярность в духе Recycler Charge из Prey:
// снаряд застревает в разрезе и бьётся в центре, пространство рвётся воронкой, врагов, трупы и физ-объекты
// затягивает внутрь, затем снаряд и всё вокруг схлопывается в точку и взрывается
// (урон — взрывом самого снаряда, x DamageMultiplier).
public class Singularity : MonoBehaviour
{
    private const float PullDuration = 1.6f;
    private const float CollapseDuration = 0.18f;
    private const float PullRadius = 13f;
    private const float VisualRadius = 4.5f;
    private const float OpenTime = 0.2f;
    private const float PullSpeed = 9f;
    private const float PullAcceleration = 60f;
    private const float DamageMultiplier = 4f;
    private const float ExplosionRadius = 11f;
    private const float CollapseTimeStop = 0.18f;

    private const int StreakCount = 40;
    private const int RingPoints = 40;

    private static readonly Collider[] Buffer = new Collider[128];
    private static readonly Color Violet = new Color(0.55f, 0.25f, 1f);
    private static readonly int DistortionStrengthId = Shader.PropertyToID("_DistortionStrength");

    private struct Streak
    {
        public LineRenderer Line;
        public Vector3 Direction;
        public float Radius;
        public float Speed;
    }

    private readonly HashSet<Object> _pulled = new HashSet<Object>();

    private Projectile _projectile;
    private Vector3 _projectileScale;
    private Vector3 _spinAxis;
    private Vector3 _center;
    private float _time;
    private Color _color;
    private Camera _camera;

    private Material _glowMaterial;
    private Material _distortionMaterial;
    private Transform[] _shells;
    private Transform[] _rings;
    private Transform _billboard;
    private LineRenderer _billboardHaze;
    private LineRenderer _core;
    private Streak[] _streaks;
    private Light _light;

    public static void Trigger(Projectile projectile)
    {
        if (projectile == null || projectile.SingularityCaptured)
            return;

        Vector3 center = projectile.transform.position;
        projectile.Capture(PullDuration + CollapseDuration + 1f);

        GameObject root = new GameObject("Singularity");
        root.transform.position = center;
        root.AddComponent<Singularity>().Begin(projectile);
    }

    private void Begin(Projectile projectile)
    {
        _projectile = projectile;
        _projectileScale = projectile.transform.localScale;
        _spinAxis = Random.onUnitSphere;
        _center = transform.position;
        _color = Color.Lerp(LevelPalette.Color, Violet, 0.35f);

        Build();
        RiftCamera.Acquire();
        ExplosionPostFx.Punch(0.6f, _color, 0.45f);
        ComboFx.FlashLight(_center, _color, 12f, 18f, 0.3f);
    }

    private void OnDestroy()
    {
        RiftCamera.Release();
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        _time += deltaTime;

        if (_time >= PullDuration + CollapseDuration)
        {
            Collapse();
            return;
        }

        bool collapsing = _time > PullDuration;
        float pullProgress = Mathf.Clamp01(_time / PullDuration);
        float strength = Mathf.Lerp(0.4f, 1.6f, pullProgress * pullProgress);

        if (!collapsing)
        {
            Pull(deltaTime, strength);
            // Экран "засасывает" (линза внутрь) — сильнее к концу.
            ExplosionPostFx.Punch(0.25f + 0.35f * pullProgress, _color, 0.45f);
        }

        Animate(deltaTime, collapsing);
    }

    private void Pull(float deltaTime, float strength)
    {
        _pulled.Clear();
        int count = Physics.OverlapSphereNonAlloc(_center, PullRadius, Buffer, ~0, QueryTriggerInteraction.Collide);
        for (int i = 0; i < count; i++)
        {
            Collider other = Buffer[i];
            if (other == null || other.GetComponentInParent<PlayerMovement>() != null
                || other.GetComponentInParent<PlayerSlashProjectile>() != null
                || other.GetComponentInParent<Projectile>() != null)
                continue;

            NavMeshAgent agent = other.GetComponentInParent<NavMeshAgent>();
            Rigidbody body = other.attachedRigidbody;
            Object key = agent != null ? (Object)agent : body;
            if (key == null || !_pulled.Add(key))
                continue;

            Vector3 position = agent != null ? agent.transform.position : body.worldCenterOfMass;
            Vector3 toCenter = _center - position;
            float distance = toCenter.magnitude;
            if (distance < 0.8f)
                continue;

            Vector3 direction = toCenter / distance;
            float falloff = 0.4f + (1f - distance / PullRadius);

            if (agent != null && agent.enabled && agent.isOnNavMesh)
                agent.Move(direction * (PullSpeed * strength * falloff * deltaTime));
            else if (body != null && !body.isKinematic)
                body.AddForce(direction * (PullAcceleration * strength * falloff), ForceMode.Acceleration);
        }
    }

    private void Collapse()
    {
        if (_projectile != null && _projectile.isActiveAndEnabled)
        {
            _projectile.transform.localScale = _projectileScale;
            _projectile.transform.position = _center;
            _projectile.ExplosionRange = ExplosionRadius;
            _projectile.Explode(DamageMultiplier);
        }
        else
        {
            ExplosionFx.Play(_center, ExplosionRadius, ExplosionFx.Kind.Singularity);
        }

        if (PlayerTime.Instance != null)
            PlayerTime.Instance.TimeStop(CollapseTimeStop);

        Destroy(gameObject);
    }

    #region Visuals

    private void Animate(float deltaTime, bool collapsing)
    {
        float open = Mathf.Clamp01(_time / OpenTime);
        float collapse = collapsing ? 1f - Mathf.Clamp01((_time - PullDuration) / CollapseDuration) : 1f;
        float wobble = 1f + Mathf.Sin(_time * 22f) * 0.05f;
        float radius = VisualRadius * EaseOut(open) * collapse * collapse * wobble;

        foreach (Transform shell in _shells)
        {
            shell.localScale = Vector3.one * radius;
            shell.Rotate(Vector3.up, 40f * deltaTime, Space.World);
        }

        for (int i = 0; i < _rings.Length; i++)
        {
            _rings[i].localScale = Vector3.one * (radius * 0.75f);
            _rings[i].Rotate(i == 1 ? Vector3.right : Vector3.forward, (300f + 500f * _time) * deltaTime, Space.Self);
        }

        if (_camera == null)
            _camera = Camera.main;
        if (_camera != null)
            _billboard.rotation = Quaternion.LookRotation(_camera.transform.position - _billboard.position);
        _billboard.localScale = Vector3.one * (radius * 1.25f);
        _core.widthMultiplier = 0.5f + 1.5f * (1f - collapse) + 0.3f * Mathf.Sin(_time * 30f);

        if (_distortionMaterial != null && _distortionMaterial.HasProperty(DistortionStrengthId))
            _distortionMaterial.SetFloat(DistortionStrengthId, 5f + 6f * Mathf.Clamp01(_time / PullDuration));

        for (int i = 0; i < _streaks.Length; i++)
            UpdateStreak(ref _streaks[i], deltaTime, collapse);

        _light.intensity = Mathf.Lerp(3f, 16f, Mathf.Clamp01(_time / PullDuration)) + (collapsing ? 20f : 0f);
        _light.range = PullRadius * 1.5f;

        AnimateProjectile(deltaTime, collapse);
    }

    // Снаряд бьётся в центре: крутится, дрожит всё сильнее, на схлопывании сжимается в точку.
    private void AnimateProjectile(float deltaTime, float collapse)
    {
        if (_projectile == null || !_projectile.isActiveAndEnabled)
            return;

        float progress = Mathf.Clamp01(_time / PullDuration);
        Transform target = _projectile.transform;
        target.position = _center + Random.insideUnitSphere * (0.05f + 0.35f * progress) * collapse;
        target.Rotate(_spinAxis, (200f + 900f * progress) * deltaTime, Space.World);
        float pulse = 1f + Mathf.Sin(_time * 25f) * 0.08f * progress;
        target.localScale = _projectileScale * (pulse * collapse);
    }

    private void UpdateStreak(ref Streak streak, float deltaTime, float collapse)
    {
        float pull = Mathf.Lerp(1f, 5f, 1f - streak.Radius / PullRadius);
        streak.Radius -= streak.Speed * pull * deltaTime;
        if (streak.Radius <= 0.4f)
            ResetStreak(ref streak);

        float tail = Mathf.Min(streak.Radius + 0.6f + pull * 0.5f, PullRadius);
        streak.Line.SetPosition(0, _center + streak.Direction * tail);
        streak.Line.SetPosition(1, _center + streak.Direction * streak.Radius);
        streak.Line.widthMultiplier = 0.1f * collapse;
    }

    private static void ResetStreak(ref Streak streak)
    {
        streak.Direction = Random.onUnitSphere;
        streak.Radius = Random.Range(PullRadius * 0.6f, PullRadius);
        streak.Speed = Random.Range(4f, 8f);
    }

    private void Build()
    {
        ComboAssets assets = ComboAssets.Instance;
        if (assets != null && assets.RiftGlowMaterial != null)
            _glowMaterial = new Material(assets.RiftGlowMaterial) { renderQueue = 3010 };
        if (assets != null && assets.RiftDistortionMaterial != null)
            _distortionMaterial = new Material(assets.RiftDistortionMaterial) { renderQueue = 3000 };

        Color hot = Color.Lerp(_color, Color.white, 0.5f);

        // Оболочка искажения из трёх перпендикулярных колец — "пузырь", который гнёт пространство.
        _shells = new Transform[3];
        for (int i = 0; i < 3; i++)
        {
            Transform shell = new GameObject("Shell").transform;
            shell.SetParent(transform, false);
            shell.localRotation = Quaternion.Euler(i == 1 ? 90f : 0f, i == 2 ? 90f : 0f, 0f);
            LineRenderer line = CreateCircle(shell, "Distortion", _distortionMaterial, 1f, 0.9f, Color.white);
            line.enabled = _distortionMaterial != null;
            _shells[i] = shell;
        }

        _rings = new Transform[3];
        for (int i = 0; i < 3; i++)
        {
            Transform ring = new GameObject("Ring").transform;
            ring.SetParent(transform, false);
            ring.localRotation = Quaternion.Euler(i * 55f, i * 35f, 0f);
            CreateCircle(ring, "Glow", _glowMaterial, 1f, 0.05f, hot);
            _rings[i] = ring;
        }

        _billboard = new GameObject("Billboard").transform;
        _billboard.SetParent(transform, false);
        _billboardHaze = CreateCircle(_billboard, "Haze", _distortionMaterial, 1f, 1.2f, Color.white);
        _billboardHaze.enabled = _distortionMaterial != null;
        _core = CreateCircle(_billboard, "Core", _glowMaterial, 0.12f, 1f, Color.white);

        _streaks = new Streak[StreakCount];
        for (int i = 0; i < StreakCount; i++)
        {
            LineRenderer line = CreateLine(transform, "Streak", _glowMaterial, 2, false, true);
            line.widthCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
            line.startColor = new Color(hot.r, hot.g, hot.b, 0f);
            line.endColor = hot;
            _streaks[i].Line = line;
            ResetStreak(ref _streaks[i]);
        }

        _light = gameObject.AddComponent<Light>();
        _light.type = LightType.Point;
        _light.shadows = LightShadows.None;
        _light.color = _color;

        if (assets != null && assets.RiftSparks != null)
        {
            GameObject sparks = Instantiate(assets.RiftSparks, transform);
            sparks.transform.localPosition = Vector3.zero;
            sparks.transform.localScale = Vector3.one * 2f;
        }
    }

    private static LineRenderer CreateCircle(Transform parent, string name, Material material, float radius,
        float width, Color color)
    {
        LineRenderer line = CreateLine(parent, name, material, RingPoints, true, false);
        for (int i = 0; i < RingPoints; i++)
        {
            float angle = i * Mathf.PI * 2f / RingPoints;
            line.SetPosition(i, new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * radius);
        }

        line.widthMultiplier = width;
        line.startColor = color;
        line.endColor = color;
        return line;
    }

    private static LineRenderer CreateLine(Transform parent, string name, Material material, int points, bool loop,
        bool worldSpace)
    {
        GameObject lineObject = new GameObject(name);
        lineObject.transform.SetParent(parent, false);
        LineRenderer line = lineObject.AddComponent<LineRenderer>();
        line.useWorldSpace = worldSpace;
        line.loop = loop;
        line.positionCount = points;
        line.sharedMaterial = material;
        line.textureMode = LineTextureMode.Stretch;
        line.numCornerVertices = 2;
        line.shadowCastingMode = ShadowCastingMode.Off;
        line.receiveShadows = false;
        line.enabled = material != null;
        return line;
    }

    private static float EaseOut(float t)
    {
        return 1f - (1f - t) * (1f - t);
    }

    #endregion
}
