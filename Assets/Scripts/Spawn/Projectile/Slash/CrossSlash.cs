using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

// Волна РПГ — ровный крест-разлом пространства.
//   Центр: воронка искажения экрана, в которую по спирали затягивает искры и обломки "реальности".
//   Рукава: рваные разрывы — искажение, тёмная сердцевина, светящиеся края, глитч-дубли.
//   Открытие: ударная волна искажения и удар по экрану; в конце жизни разлом схлопывается.
// Крест всегда стоит ровно (горизонталь + вертикаль), летит туда, куда целился игрок, и проходит сквозь всё.
// Вдали линии и искажение усиливаются по расстоянию, чтобы разлом не терялся.
public class CrossSlash : MonoBehaviour
{
    private const string ArmName = "CrossArm";
    private const string RiftName = "Rift";

    private const int RiftPoints = 28;
    private const int RingPoints = 48;
    private const int StreakCount = 14;
    private const int ShardCount = 8;
    private const float ArmLengthFactor = 0.9f;
    private const float GlitchInterval = 0.06f;

    private const float FunnelRadius = 2.2f;
    private const float LensRadius = 3.6f;
    private const float StreakOuterRadius = 10f;
    private const float ShockwaveTime = 0.35f;
    private const float ShockwaveRadius = 14f;
    private const float CloseTime = 0.6f;
    private const float DistanceForFullWidth = 12f;
    private const float MaxDistanceBoost = 6f;
    private const float DistortionStrength = 4f;
    private const float OpenDuration = 0.6f;

    private const float BarDistortionWidth = 5f;
    private const float BarCoreWidth = 0.7f;
    private const float BarEdgeWidth = 0.22f;
    private const float BarGlowWidth = 1.8f;
    private const float ChromaWidth = 0.14f;
    private const float LensWidth = 4.5f;

    private const int QueueDistortion = 3000;
    private const int QueueGlow = 3010;
    private const int QueueCore = 3020;

    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    private static readonly int ColorId = Shader.PropertyToID("_Color");
    private static readonly int DistortionStrengthId = Shader.PropertyToID("_DistortionStrength");

    private static readonly Color Magenta = new Color(1f, 0.2f, 0.85f);
    private static readonly Color Cyan = new Color(0.2f, 1f, 1f);
    private static readonly Color Violet = new Color(0.55f, 0.25f, 1f);

    private class Bar
    {
        public Vector3 Axis;
        public Vector3 Side;
        public Vector3[] Path;
        public Vector3[] Buffer;
        public LineRenderer Distortion;
        public LineRenderer Glow;
        public LineRenderer EdgeA;
        public LineRenderer EdgeB;
        public LineRenderer Core;
        public LineRenderer ChromaA;
        public LineRenderer ChromaB;
    }

    private struct Streak
    {
        public LineRenderer Line;
        public float Angle;
        public float Radius;
        public float Speed;
    }

    private struct Shard
    {
        public LineRenderer Line;
        public float Angle;
        public float Radius;
        public float Spin;
        public float Size;
        public float Orbit;
    }

    private Bar[] _bars;
    private Streak[] _streaks;
    private Shard[] _shards;
    private LineRenderer _lens;
    private LineRenderer _shockwave;
    private TrailRenderer[] _tipTrails;
    private Renderer[] _hiddenRenderers;
    private Light _light;

    private Material _glowMaterial;
    private Material _distortionMaterial;
    private Material _coreMaterial;

    private Vector3 _center;
    private float _halfLength;
    private float _jitter;
    private float _size = 1f;
    private Vector3 _lastApplied;
    private Color _color;
    private float _time;
    private float _lifetime = 10f;
    private float _widthScale = 1f;
    private float _distanceBoost = 1f;
    private Camera _camera;
    private float _nextGlitch;
    private bool _cameraAcquired;
    private readonly Vector3[] _ring = new Vector3[RingPoints];

    public static void Apply(PlayerSlashProjectile slash, float size, Color color, float lifetime)
    {
        CrossSlash cross = slash.GetComponent<CrossSlash>();
        if (cross == null)
            cross = slash.gameObject.AddComponent<CrossSlash>();
        cross.Setup(size, color, lifetime);
    }

    private void Setup(float size, Color color, float lifetime)
    {
        Build();

        _size = size;
        _color = color;
        _lifetime = Mathf.Max(lifetime, CloseTime);
        _lastApplied = Vector3.zero;
        _time = 0f;

        if (TryGetComponent(out PoolObject poolObject))
            poolObject.RestartLifetime(_lifetime);

        // Скорость уже задана при запуске, поворот на неё не влияет: ставим крест ровно,
        // без крена камеры и наклона по взгляду — горизонталь горизонтальна, вертикаль вертикальна.
        Vector3 flatForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
        if (flatForward.sqrMagnitude < 0.0001f)
            flatForward = Vector3.ProjectOnPlane(transform.up, Vector3.up);
        transform.rotation = Quaternion.LookRotation(flatForward.normalized, Vector3.up);

        foreach (Renderer hidden in _hiddenRenderers)
            hidden.enabled = false;

        foreach (Bar bar in _bars)
            GeneratePath(bar);

        for (int i = 0; i < _streaks.Length; i++)
            ResetStreak(ref _streaks[i], Random.Range(FunnelRadius, StreakOuterRadius));
        for (int i = 0; i < _shards.Length; i++)
            ResetShard(ref _shards[i], Random.Range(LensRadius, StreakOuterRadius));

        ApplyColors();

        foreach (TrailRenderer trail in _tipTrails)
            trail.Clear();

        _light.color = Saturate(Color.Lerp(color, Violet, 0.35f));
        _light.intensity = 9f;
        _light.range = 22f;

        AcquireCamera();
        ExplosionPostFx.Punch(0.7f, Saturate(color));
        Glitch();
        Animate();
    }

    private void OnDisable()
    {
        ReleaseCamera();
    }

    private void LateUpdate()
    {
        // Аниматор волны пишет масштаб 0.1 → 1 каждый кадр — домножаем поверх, не накапливая.
        Vector3 scale = transform.localScale;
        if (scale != _lastApplied)
        {
            _lastApplied = scale * _size;
            transform.localScale = _lastApplied;
        }

        if (_bars == null)
            return;

        _time += Time.deltaTime;
        UpdateWidthScale();
        if (Time.unscaledTime >= _nextGlitch)
            Glitch();
        Animate();
    }

    // Ширина линий задана в метрах, а сила искажения у шейдера делится на глубину — вдали разлом
    // превращался в пару пикселей. Компенсируем расстоянием; плюс раскрытие в начале и схлопывание в конце.
    private void UpdateWidthScale()
    {
        if (_camera == null)
            _camera = Camera.main;

        _distanceBoost = 1f;
        if (_camera != null)
        {
            float distance = Vector3.Distance(_camera.transform.position, transform.TransformPoint(_center));
            _distanceBoost = Mathf.Clamp(distance / DistanceForFullWidth, 1f, MaxDistanceBoost);
        }

        float closing = Mathf.Clamp01((_lifetime - _time) / CloseTime);
        _widthScale = Open * closing * _distanceBoost;

        if (_distortionMaterial != null && _distortionMaterial.HasProperty(DistortionStrengthId))
            _distortionMaterial.SetFloat(DistortionStrengthId, DistortionStrength * _distanceBoost * closing);
    }

    #region Animation

    // Раскрытие по времени, а не по масштабу объекта: чужие изменения масштаба не спрячут разлом.
    private float Open => Mathf.SmoothStep(0.1f, 1f, Mathf.Clamp01(_time / OpenDuration));

    private void Animate()
    {
        float scale = _widthScale;
        float deltaTime = Time.deltaTime;

        foreach (Bar bar in _bars)
        {
            bar.Distortion.widthMultiplier = BarDistortionWidth * scale;
            bar.Core.widthMultiplier = BarCoreWidth * scale;
        }

        // Воронка искажения в перекрестье "дышит".
        SetCircle(_lens, LensRadius * (1f + Mathf.Sin(_time * 4f) * 0.08f), 0f, 1f);
        _lens.widthMultiplier = LensWidth * scale;

        foreach (TrailRenderer trail in _tipTrails)
            trail.widthMultiplier = 0.7f * scale;

        for (int i = 0; i < _streaks.Length; i++)
            UpdateStreak(ref _streaks[i], deltaTime, scale);
        for (int i = 0; i < _shards.Length; i++)
            UpdateShard(ref _shards[i], deltaTime, scale);

        UpdateShockwave();
    }

    // Искры по спирали затягивает в ядро; у края горизонта они вытягиваются и гаснут.
    private void UpdateStreak(ref Streak streak, float deltaTime, float open)
    {
        float pull = Mathf.Lerp(1f, 4f, 1f - Mathf.InverseLerp(FunnelRadius, StreakOuterRadius, streak.Radius));
        streak.Radius -= streak.Speed * pull * deltaTime;
        streak.Angle += pull * 1.6f * deltaTime;
        if (streak.Radius <= FunnelRadius)
            ResetStreak(ref streak, StreakOuterRadius);

        float tailRadius = Mathf.Min(streak.Radius + 0.4f + pull * 0.35f, StreakOuterRadius + 1f);
        streak.Line.SetPosition(0, PolarPoint(streak.Angle - 0.15f, tailRadius));
        streak.Line.SetPosition(1, PolarPoint(streak.Angle, streak.Radius));

        float fade = Mathf.InverseLerp(StreakOuterRadius, StreakOuterRadius * 0.7f, streak.Radius);
        streak.Line.widthMultiplier = 0.12f * open * Mathf.Clamp01(fade + 0.2f);
    }

    // Обломки "реальности" — треугольные осколки на орбите, медленно падающие в дыру.
    private void UpdateShard(ref Shard shard, float deltaTime, float open)
    {
        shard.Radius -= 0.6f * deltaTime;
        shard.Angle += shard.Orbit * deltaTime;
        if (shard.Radius <= FunnelRadius * 1.2f)
            ResetShard(ref shard, StreakOuterRadius);

        Vector3 center = PolarPoint(shard.Angle, shard.Radius);
        float rotation = _time * shard.Spin;
        for (int i = 0; i < 3; i++)
        {
            float corner = rotation + i * Mathf.PI * 2f / 3f;
            Vector3 offset = new Vector3(Mathf.Cos(corner), Mathf.Sin(corner), 0f) * shard.Size;
            shard.Line.SetPosition(i, center + offset);
        }

        shard.Line.widthMultiplier = 0.06f * open;
        shard.Line.enabled = Random.value > 0.08f;
    }

    private void UpdateShockwave()
    {
        float progress = _time / ShockwaveTime;
        bool active = progress < 1f;
        _shockwave.enabled = active && _distortionMaterial != null;
        if (!active)
            return;

        float eased = 1f - (1f - progress) * (1f - progress);
        SetCircle(_shockwave, Mathf.Lerp(FunnelRadius, ShockwaveRadius, eased), 0f, 1f);
        _shockwave.widthMultiplier = Mathf.Lerp(6f, 0.5f, progress) * _distanceBoost;
    }

    private void Glitch()
    {
        _nextGlitch = Time.unscaledTime + GlitchInterval * Random.Range(0.6f, 1.4f);
        float open = _widthScale;

        foreach (Bar bar in _bars)
        {
            SetOffset(bar.Core, bar, 0f, _jitter * 0.05f);
            SetOffset(bar.EdgeA, bar, _jitter * 0.12f, _jitter * 0.06f);
            SetOffset(bar.EdgeB, bar, -_jitter * 0.12f, _jitter * 0.06f);
            SetOffset(bar.Glow, bar, 0f, _jitter * 0.1f);
            bar.EdgeA.widthMultiplier = BarEdgeWidth * open * Random.Range(0.7f, 1.3f);
            bar.EdgeB.widthMultiplier = BarEdgeWidth * open * Random.Range(0.7f, 1.3f);
            bar.Glow.widthMultiplier = BarGlowWidth * open * Random.Range(0.8f, 1.2f);

            bar.ChromaA.enabled = Random.value < 0.7f;
            bar.ChromaB.enabled = Random.value < 0.7f;
            SetOffset(bar.ChromaA, bar, Random.Range(0.25f, 0.6f), 0f);
            SetOffset(bar.ChromaB, bar, -Random.Range(0.25f, 0.6f), 0f);
            bar.ChromaA.widthMultiplier = ChromaWidth * open;
            bar.ChromaB.widthMultiplier = ChromaWidth * open;
        }
    }

    #endregion

    #region Building

    private void Build()
    {
        if (_bars != null)
            return;

        BoxCollider sourceCollider = GetComponent<BoxCollider>();
        _center = sourceCollider != null ? new Vector3(0f, 0f, sourceCollider.center.z) : Vector3.zero;
        _halfLength = sourceCollider != null ? sourceCollider.size.x * 0.5f * ArmLengthFactor : 12f;
        _jitter = sourceCollider != null ? sourceCollider.size.y * 0.12f : 0.8f;

        Transform armTransform = transform.Find(ArmName);
        if (armTransform == null)
            armTransform = CreateArmCollider(sourceCollider).transform;
        _hiddenRenderers = System.Array.FindAll(
            new[] { GetComponent<Renderer>(), armTransform.GetComponent<Renderer>() }, target => target != null);

        CreateMaterials();

        Transform rift = new GameObject(RiftName).transform;
        rift.SetParent(transform, false);

        _bars = new[]
        {
            CreateBar(rift, Vector3.right, Vector3.up),
            CreateBar(rift, Vector3.up, Vector3.right),
        };

        _lens = CreateLine(rift, "Lens", _distortionMaterial, LensWidth, RingPoints, true);
        _lens.enabled = _distortionMaterial != null;
        _shockwave = CreateLine(rift, "Shockwave", _distortionMaterial, 6f, RingPoints, true);

        _streaks = new Streak[StreakCount];
        for (int i = 0; i < StreakCount; i++)
            _streaks[i].Line = CreateLine(rift, "Streak", _glowMaterial, 0.12f, 2, false);

        _shards = new Shard[ShardCount];
        for (int i = 0; i < ShardCount; i++)
        {
            _shards[i].Line = CreateLine(rift, "Shard", _glowMaterial, 0.06f, 3, true);
            _shards[i].Line.widthCurve = AnimationCurve.Constant(0f, 1f, 1f);
        }

        _tipTrails = new[]
        {
            CreateTipTrail(rift, _center + Vector3.right * _halfLength),
            CreateTipTrail(rift, _center - Vector3.right * _halfLength),
            CreateTipTrail(rift, _center + Vector3.up * _halfLength),
            CreateTipTrail(rift, _center - Vector3.up * _halfLength),
        };

        Transform lightTransform = new GameObject("CrossLight").transform;
        lightTransform.SetParent(transform, false);
        lightTransform.localPosition = _center;
        _light = lightTransform.gameObject.AddComponent<Light>();
        _light.type = LightType.Point;
        _light.shadows = LightShadows.None;

        ComboAssets assets = ComboAssets.Instance;
        if (assets != null && assets.RiftSparks != null)
        {
            GameObject sparks = Instantiate(assets.RiftSparks, transform);
            sparks.transform.localPosition = _center;
            sparks.transform.localRotation = Quaternion.identity;
            sparks.transform.localScale = Vector3.one * 3f;
        }

    }

    // Свои экземпляры материалов: нужен порядок отрисовки (искажение → свечение → чёрное ядро)
    // и более сильное искажение, общие ассеты при этом не меняются.
    private void CreateMaterials()
    {
        ComboAssets assets = ComboAssets.Instance;
        Renderer source = GetComponent<Renderer>();

        if (assets != null && assets.RiftGlowMaterial != null)
        {
            _glowMaterial = new Material(assets.RiftGlowMaterial) { renderQueue = QueueGlow };
        }

        if (assets != null && assets.RiftDistortionMaterial != null)
        {
            _distortionMaterial = new Material(assets.RiftDistortionMaterial) { renderQueue = QueueDistortion };
            if (_distortionMaterial.HasProperty(DistortionStrengthId))
                _distortionMaterial.SetFloat(DistortionStrengthId, 4f);
        }

        Material coreSource = source != null ? source.sharedMaterial : _glowMaterial;
        if (coreSource != null)
        {
            _coreMaterial = new Material(coreSource) { renderQueue = QueueCore };
            if (_coreMaterial.HasProperty(BaseColorId))
                _coreMaterial.SetColor(BaseColorId, Color.black);
            if (_coreMaterial.HasProperty(ColorId))
                _coreMaterial.SetColor(ColorId, Color.black);
        }
    }

    private GameObject CreateArmCollider(BoxCollider sourceCollider)
    {
        GameObject arm = new GameObject(ArmName);
        arm.layer = gameObject.layer;
        arm.transform.SetParent(transform, false);
        arm.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);

        if (sourceCollider != null)
        {
            BoxCollider armCollider = arm.AddComponent<BoxCollider>();
            armCollider.size = sourceCollider.size;
            armCollider.center = sourceCollider.center;
            armCollider.isTrigger = sourceCollider.isTrigger;
        }

        return arm;
    }

    private Bar CreateBar(Transform parent, Vector3 axis, Vector3 side)
    {
        Bar bar = new Bar
        {
            Axis = axis,
            Side = side,
            Path = new Vector3[RiftPoints],
            Buffer = new Vector3[RiftPoints],
        };

        bar.Distortion = CreateLine(parent, "Distortion", _distortionMaterial, BarDistortionWidth, RiftPoints, false);
        bar.Distortion.enabled = _distortionMaterial != null;
        bar.Glow = CreateLine(parent, "Glow", _glowMaterial, BarGlowWidth, RiftPoints, false);
        bar.ChromaA = CreateLine(parent, "ChromaA", _glowMaterial, ChromaWidth, RiftPoints, false);
        bar.ChromaB = CreateLine(parent, "ChromaB", _glowMaterial, ChromaWidth, RiftPoints, false);
        bar.EdgeA = CreateLine(parent, "EdgeA", _glowMaterial, BarEdgeWidth, RiftPoints, false);
        bar.EdgeB = CreateLine(parent, "EdgeB", _glowMaterial, BarEdgeWidth, RiftPoints, false);
        bar.Core = CreateLine(parent, "Void", _coreMaterial, BarCoreWidth, RiftPoints, false);
        return bar;
    }

    private static LineRenderer CreateLine(Transform parent, string name, Material material, float width,
        int points, bool loop)
    {
        GameObject lineObject = new GameObject(name);
        lineObject.transform.SetParent(parent, false);
        LineRenderer line = lineObject.AddComponent<LineRenderer>();
        line.useWorldSpace = false;
        line.loop = loop;
        line.positionCount = points;
        line.sharedMaterial = material;
        line.widthMultiplier = width;
        if (!loop)
        {
            line.widthCurve = new AnimationCurve(
                new Keyframe(0f, 0f), new Keyframe(0.1f, 0.55f), new Keyframe(0.5f, 1f),
                new Keyframe(0.9f, 0.55f), new Keyframe(1f, 0f));
        }

        line.textureMode = LineTextureMode.Stretch;
        line.numCornerVertices = 2;
        line.shadowCastingMode = ShadowCastingMode.Off;
        line.receiveShadows = false;
        line.enabled = material != null;
        return line;
    }

    private TrailRenderer CreateTipTrail(Transform parent, Vector3 localPosition)
    {
        GameObject tip = new GameObject("TipTrail");
        tip.transform.SetParent(parent, false);
        tip.transform.localPosition = localPosition;
        TrailRenderer trail = tip.AddComponent<TrailRenderer>();
        trail.sharedMaterial = _glowMaterial;
        trail.time = 0.3f;
        trail.widthMultiplier = 0.7f;
        trail.widthCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 0f));
        trail.minVertexDistance = 0.3f;
        trail.shadowCastingMode = ShadowCastingMode.Off;
        return trail;
    }

    #endregion

    #region Shapes

    private void GeneratePath(Bar bar)
    {
        for (int i = 0; i < RiftPoints; i++)
        {
            float t = (float)i / (RiftPoints - 1);
            float along = Mathf.Lerp(-_halfLength, _halfLength, t);

            // У центра разрыв уходит в воронку — там он ровнее, к концам рвётся сильнее.
            float distanceFromCenter = Mathf.InverseLerp(FunnelRadius, _halfLength, Mathf.Abs(along));
            float offset = Random.Range(-_jitter, _jitter) * Mathf.Sin(t * Mathf.PI) * distanceFromCenter;
            bar.Path[i] = _center + bar.Axis * along + bar.Side * offset;
        }

        bar.Distortion.SetPositions(bar.Path);
    }

    private void SetOffset(LineRenderer line, Bar bar, float shift, float noise)
    {
        for (int i = 0; i < RiftPoints; i++)
            bar.Buffer[i] = bar.Path[i] + bar.Side * (shift + Random.Range(-noise, noise));
        line.SetPositions(bar.Buffer);
    }

    private void SetCircle(LineRenderer line, float radius, float phase, float flatten)
    {
        for (int i = 0; i < RingPoints; i++)
        {
            float angle = phase + i * Mathf.PI * 2f / RingPoints;
            _ring[i] = _center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius * flatten, 0f);
        }

        line.SetPositions(_ring);
    }

    private Vector3 PolarPoint(float angle, float radius)
    {
        return _center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * radius;
    }

    private static void ResetStreak(ref Streak streak, float radius)
    {
        streak.Radius = radius;
        streak.Angle = Random.Range(0f, Mathf.PI * 2f);
        streak.Speed = Random.Range(2.5f, 5f);
    }

    private static void ResetShard(ref Shard shard, float radius)
    {
        shard.Radius = radius;
        shard.Angle = Random.Range(0f, Mathf.PI * 2f);
        shard.Spin = Random.Range(-6f, 6f);
        shard.Size = Random.Range(0.25f, 0.6f);
        shard.Orbit = Random.Range(0.4f, 1.2f) * (Random.value < 0.5f ? -1f : 1f);
    }

    private void ApplyColors()
    {
        Color glow = Saturate(_color);
        Color hot = Color.Lerp(glow, Color.white, 0.6f);

        foreach (Bar bar in _bars)
        {
            SetColor(bar.Glow, new Color(glow.r, glow.g, glow.b, 0.45f), new Color(glow.r, glow.g, glow.b, 0.2f));
            SetColor(bar.EdgeA, hot, glow);
            SetColor(bar.EdgeB, hot, glow);
            SetColor(bar.ChromaA, Magenta, new Color(Magenta.r, Magenta.g, Magenta.b, 0.4f));
            SetColor(bar.ChromaB, Cyan, new Color(Cyan.r, Cyan.g, Cyan.b, 0.4f));
            SetColor(bar.Core, Color.black, Color.black);
            SetColor(bar.Distortion, Color.white, Color.white);
        }

        SetColor(_lens, Color.white, Color.white);
        SetColor(_shockwave, Color.white, Color.white);

        foreach (Streak streak in _streaks)
            SetColor(streak.Line, new Color(hot.r, hot.g, hot.b, 0f), hot);
        foreach (Shard shard in _shards)
            SetColor(shard.Line, Color.Lerp(Cyan, Color.white, 0.5f), Magenta);

        foreach (TrailRenderer trail in _tipTrails)
        {
            trail.startColor = glow;
            trail.endColor = new Color(glow.r, glow.g, glow.b, 0f);
        }
    }

    private static void SetColor(LineRenderer line, Color start, Color end)
    {
        line.startColor = start;
        line.endColor = end;
    }

    private static Color Saturate(Color color)
    {
        return new Color(Mathf.Clamp01(color.r), Mathf.Clamp01(color.g), Mathf.Clamp01(color.b), 1f);
    }

    #endregion

    #region Camera

    private void AcquireCamera()
    {
        if (_cameraAcquired)
            return;
        _cameraAcquired = true;
        RiftCamera.Acquire();
    }

    private void ReleaseCamera()
    {
        if (!_cameraAcquired)
            return;
        _cameraAcquired = false;
        RiftCamera.Release();
    }

    #endregion
}
