using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

// Визуал шара рейлгана — сгусток энергии поверх родного меша и молний:
//   ядро — яркое CFXR-свечение и вихрь-спираль внутри (всегда к камере);
//   корона из мерцающих шипов, светящийся ободок и хроматические глитч-дубли (маджента/циан);
//   искажающий гало-пузырь, дуги-молнии из ядра наружу, три кольца-"гироскопа";
//   электроны на орбитах с кометными шлейфами, энергетический шлейф и "горячий" след искажения позади;
//   глитч-кадры — шар на мгновение дёргается и расслаивается по цветам.
// SetCharge (0..1) — по мере "раздувания" цвет уходит к цвету уровня, всё длиннее, быстрее и ярче.
public class EnergyOrb : MonoBehaviour
{
    private const string OrbName = "EnergyOrb";
    private const int ArcCount = 8;
    private const int ArcPoints = 10;
    private const int RingPoints = 40;
    private const int SpikeCount = 16;
    private const int SwirlPoints = 48;
    private const int ElectronCount = 5;
    private const float ArcInterval = 0.05f;
    private const float SpikeInterval = 0.04f;

    private static readonly Color BaseColor = new Color(0.35f, 0.9f, 1f);
    private static readonly Color Magenta = new Color(1f, 0.2f, 0.85f);
    private static readonly Color Cyan = new Color(0.2f, 1f, 1f);
    private static readonly int DistortionStrengthId = Shader.PropertyToID("_DistortionStrength");

    private LineRenderer[] _arcs;
    private Transform[] _rings;
    private LineRenderer[] _ringLines;
    private Transform _halo;
    private Transform _swirlRoot;
    private LineRenderer _swirl;
    private LineRenderer _rim;
    private LineRenderer _haze;
    private LineRenderer[] _chromaRims;
    private LineRenderer[] _spikes;
    private Transform[] _electrons;
    private TrailRenderer[] _electronTrails;
    private TrailRenderer _wake;
    private TrailRenderer _heatWake;
    private Transform _coreGlow;
    private Light _light;
    private Material _distortionMaterial;
    private Camera _camera;

    private float _radius = 2f;
    private float _charge;
    private float _time;
    private float _nextArcs;
    private float _nextSpikes;
    private float _nextGlitch;
    private float _glitchUntil;
    private Vector3 _glitchOffset;
    private float _flash;
    private bool _cameraAcquired;
    private readonly Vector3[] _arcBuffer = new Vector3[ArcPoints];

    public static EnergyOrb Attach(Transform ball, float radius)
    {
        Transform existing = ball.Find(OrbName);
        EnergyOrb orb = existing != null ? existing.GetComponent<EnergyOrb>() : null;
        if (orb == null)
        {
            GameObject root = new GameObject(OrbName);
            root.transform.SetParent(ball, false);
            orb = root.AddComponent<EnergyOrb>();
            orb._radius = radius;
            orb.Build();
        }

        orb.gameObject.SetActive(true);
        orb.SetCharge(0f);
        foreach (TrailRenderer trail in orb._electronTrails)
            trail.Clear();
        orb._wake.Clear();
        orb._heatWake.Clear();
        return orb;
    }

    public void SetCharge(float charge)
    {
        _charge = Mathf.Clamp01(charge);
        Color color = CurrentColor;
        Color hot = Color.Lerp(color, Color.white, 0.55f);

        foreach (LineRenderer arc in _arcs)
            SetColor(arc, Color.white, new Color(color.r, color.g, color.b, 0.2f));

        foreach (LineRenderer ring in _ringLines)
            SetColor(ring, hot, hot);

        SetColor(_rim, new Color(hot.r, hot.g, hot.b, 0.85f), new Color(hot.r, hot.g, hot.b, 0.85f));
        SetColor(_swirl, new Color(color.r, color.g, color.b, 0f), Color.white);
        SetColor(_chromaRims[0], new Color(Magenta.r, Magenta.g, Magenta.b, 0.7f), Magenta);
        SetColor(_chromaRims[1], new Color(Cyan.r, Cyan.g, Cyan.b, 0.7f), Cyan);

        foreach (LineRenderer spike in _spikes)
            SetColor(spike, hot, new Color(color.r, color.g, color.b, 0f));

        for (int i = 0; i < _electronTrails.Length; i++)
        {
            Color electron = i % 3 == 1 ? Magenta : i % 3 == 2 ? Color.white : color;
            _electronTrails[i].startColor = electron;
            _electronTrails[i].endColor = new Color(electron.r, electron.g, electron.b, 0f);
        }

        _wake.startColor = new Color(color.r, color.g, color.b, 0.75f);
        _wake.endColor = new Color(color.r, color.g, color.b, 0f);
        _light.color = color;
    }

    // Вспышка при попадании (рост шара) — плюс внеочередной глитч-кадр.
    public void Flash()
    {
        _flash = 1f;
        RegenerateArcs();
        StartGlitch(0.1f);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private Color CurrentColor => Color.Lerp(BaseColor, LevelPalette.Color, _charge);

    private void OnEnable()
    {
        if (_cameraAcquired)
            return;
        _cameraAcquired = true;
        RiftCamera.Acquire();
    }

    private void OnDisable()
    {
        if (!_cameraAcquired)
            return;
        _cameraAcquired = false;
        RiftCamera.Release();
    }

    private void LateUpdate()
    {
        float deltaTime = Time.deltaTime;
        _time += deltaTime;
        _flash = Mathf.MoveTowards(_flash, 0f, deltaTime * 4f);

        if (_camera == null)
            _camera = Camera.main;

        // Ширина линий в метрах, а шар растёт вместе с корнем — подстраиваем под масштаб.
        float scale = transform.lossyScale.x;
        float pulse = 1f + Mathf.Sin(_time * 14f) * 0.08f + _flash * 0.4f;
        float spin = Mathf.Lerp(90f, 260f, _charge) * (1f + _flash);
        bool glitching = Time.unscaledTime < _glitchUntil;

        for (int i = 0; i < _rings.Length; i++)
        {
            _rings[i].Rotate(RingAxis(i), spin * (i % 2 == 0 ? 1f : -1.3f) * deltaTime, Space.Self);
            _ringLines[i].widthMultiplier = 0.07f * scale * pulse;
        }

        if (_camera != null)
            _halo.rotation = Quaternion.LookRotation(_camera.transform.position - _halo.position);
        _halo.localScale = Vector3.one * pulse;
        _halo.localPosition = glitching ? _glitchOffset : Vector3.zero;

        _swirlRoot.Rotate(Vector3.forward, -(spin * 2.5f) * deltaTime, Space.Self);
        _swirl.widthMultiplier = (0.12f + 0.1f * _charge) * scale;

        _rim.widthMultiplier = (0.25f + 0.3f * _flash) * scale;
        _haze.widthMultiplier = _radius * 0.9f * scale;
        if (_distortionMaterial != null && _distortionMaterial.HasProperty(DistortionStrengthId))
            _distortionMaterial.SetFloat(DistortionStrengthId, 2.5f + 3f * _charge + 4f * _flash);

        UpdateChroma(scale, glitching);
        UpdateElectrons(scale);

        if (_time >= _nextArcs)
            RegenerateArcs();
        foreach (LineRenderer arc in _arcs)
            arc.widthMultiplier = 0.09f * scale * (1f + _flash);

        if (_time >= _nextSpikes)
            RegenerateSpikes();
        foreach (LineRenderer spike in _spikes)
            spike.widthMultiplier = 0.11f * scale * (1f + _flash);

        if (Time.unscaledTime >= _nextGlitch)
            StartGlitch(Random.Range(0.04f, 0.09f));

        _wake.widthMultiplier = _radius * 1.5f * scale;
        _heatWake.widthMultiplier = _radius * 2.2f * scale;

        if (_coreGlow != null)
            _coreGlow.localScale = Vector3.one * (_radius * 0.9f * pulse);

        _light.intensity = (5f + 7f * _charge) * pulse + 12f * _flash;
        _light.range = _radius * scale * (4f + 2f * _charge);
    }

    private void StartGlitch(float duration)
    {
        _glitchUntil = Time.unscaledTime + duration;
        _nextGlitch = Time.unscaledTime + Random.Range(0.3f, 0.8f) * Mathf.Lerp(1f, 0.5f, _charge);
        _glitchOffset = Random.insideUnitSphere * (_radius * 0.12f);
    }

    // Глитч-дубли ободка: в обычные кадры чуть дрожат, в глитч-кадры разъезжаются в стороны.
    private void UpdateChroma(float scale, bool glitching)
    {
        float split = glitching ? _radius * 0.18f : _radius * 0.03f;
        for (int i = 0; i < _chromaRims.Length; i++)
        {
            LineRenderer rim = _chromaRims[i];
            rim.enabled = rim.sharedMaterial != null && (glitching || Random.value < 0.35f);
            rim.transform.localPosition = new Vector3(i == 0 ? split : -split, Random.Range(-split, split) * 0.5f, 0f);
            rim.widthMultiplier = (glitching ? 0.16f : 0.08f) * scale;
        }
    }

    private void UpdateElectrons(float scale)
    {
        float speed = Mathf.Lerp(5f, 11f, _charge);
        for (int i = 0; i < _electrons.Length; i++)
        {
            float angle = _time * speed * (1f + i * 0.17f) + i * 2.1f;
            Quaternion orbit = Quaternion.Euler(i * 60f + 25f, i * 47f, i * 33f);
            float orbitRadius = _radius * (1.2f + 0.08f * (i % 3));
            _electrons[i].localPosition = orbit * new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * orbitRadius;
            _electronTrails[i].widthMultiplier = 0.2f * scale;
        }
    }

    // Молнии: ломаные из ядра за пределы шара, каждые ~50 мс новые; с зарядом длиннее и их больше.
    private void RegenerateArcs()
    {
        _nextArcs = _time + ArcInterval * Random.Range(0.6f, 1.4f);
        int visible = Mathf.RoundToInt(Mathf.Lerp(4f, ArcCount, _charge));

        for (int a = 0; a < _arcs.Length; a++)
        {
            LineRenderer arc = _arcs[a];
            arc.enabled = arc.sharedMaterial != null && a < visible && Random.value < 0.85f;
            if (!arc.enabled)
                continue;

            Vector3 direction = Random.onUnitSphere;
            float length = _radius * Random.Range(1.1f, 1.6f + 0.6f * _charge);
            Vector3 side = Vector3.Cross(direction, Random.onUnitSphere).normalized;
            for (int i = 0; i < ArcPoints; i++)
            {
                float t = (float)i / (ArcPoints - 1);
                float wobble = Mathf.Sin(t * Mathf.PI) * _radius * 0.25f;
                Vector3 noise = (side * Random.Range(-1f, 1f) + Random.insideUnitSphere * 0.5f) * wobble;
                _arcBuffer[i] = direction * (length * t) + noise;
            }

            arc.SetPositions(_arcBuffer);
        }
    }

    // Корона: короткие шипы по краю шара (в плоскости к камере), мерцают и меняют длину.
    private void RegenerateSpikes()
    {
        _nextSpikes = _time + SpikeInterval * Random.Range(0.7f, 1.3f);
        for (int i = 0; i < _spikes.Length; i++)
        {
            LineRenderer spike = _spikes[i];
            spike.enabled = spike.sharedMaterial != null && Random.value < 0.8f;
            if (!spike.enabled)
                continue;

            float angle = (i + Random.Range(-0.3f, 0.3f)) * Mathf.PI * 2f / _spikes.Length;
            Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
            float length = _radius * Random.Range(1.15f, 1.45f + 0.35f * _charge + 0.3f * _flash);
            spike.SetPosition(0, direction * (_radius * 0.95f));
            spike.SetPosition(1, direction * length);
        }
    }

    #region Building

    private void Build()
    {
        ComboAssets assets = ComboAssets.Instance;
        Material glow = assets != null ? assets.RiftGlowMaterial : null;
        if (assets != null && assets.RiftDistortionMaterial != null)
            _distortionMaterial = new Material(assets.RiftDistortionMaterial);

        _arcs = new LineRenderer[ArcCount];
        for (int i = 0; i < ArcCount; i++)
        {
            _arcs[i] = CreateLine(transform, "Arc", glow, ArcPoints, false);
            _arcs[i].widthCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 0f));
        }

        _rings = new Transform[3];
        _ringLines = new LineRenderer[3];
        for (int i = 0; i < 3; i++)
        {
            Transform ring = new GameObject("Ring").transform;
            ring.SetParent(transform, false);
            ring.localRotation = Quaternion.Euler(i * 60f, i * 45f, i * 30f);
            _rings[i] = ring;
            _ringLines[i] = CreateLine(ring, "Line", glow, RingPoints, true);
            SetCircle(_ringLines[i], _radius * (1.05f + i * 0.08f));
        }

        _halo = new GameObject("Halo").transform;
        _halo.SetParent(transform, false);

        _haze = CreateLine(_halo, "Haze", _distortionMaterial, RingPoints, true);
        SetColor(_haze, Color.white, Color.white);
        SetCircle(_haze, _radius * 1.35f);

        _rim = CreateLine(_halo, "Rim", glow, RingPoints, true);
        SetCircle(_rim, _radius * 1.02f);

        _chromaRims = new LineRenderer[2];
        for (int i = 0; i < 2; i++)
        {
            _chromaRims[i] = CreateLine(_halo, "ChromaRim", glow, RingPoints, true);
            SetCircle(_chromaRims[i], _radius * 1.04f);
        }

        _swirlRoot = new GameObject("Swirl").transform;
        _swirlRoot.SetParent(_halo, false);
        _swirl = CreateLine(_swirlRoot, "Line", glow, SwirlPoints, false);
        _swirl.widthCurve = new AnimationCurve(new Keyframe(0f, 0.2f), new Keyframe(1f, 1f));
        for (int i = 0; i < SwirlPoints; i++)
        {
            float t = (float)i / (SwirlPoints - 1);
            float angle = t * Mathf.PI * 5f;
            float radius = Mathf.Lerp(_radius * 0.9f, _radius * 0.1f, t);
            _swirl.SetPosition(i, new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * radius);
        }

        _spikes = new LineRenderer[SpikeCount];
        for (int i = 0; i < SpikeCount; i++)
        {
            _spikes[i] = CreateLine(_halo, "Spike", glow, 2, false);
            _spikes[i].widthCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 0f));
        }

        _electrons = new Transform[ElectronCount];
        _electronTrails = new TrailRenderer[ElectronCount];
        for (int i = 0; i < ElectronCount; i++)
        {
            Transform electron = new GameObject("Electron").transform;
            electron.SetParent(transform, false);
            _electrons[i] = electron;
            _electronTrails[i] = CreateTrail(electron.gameObject, glow, 0.25f);
        }

        _wake = CreateTrail(gameObject, glow, 0.45f);
        Transform heat = new GameObject("HeatWake").transform;
        heat.SetParent(transform, false);
        _heatWake = CreateTrail(heat.gameObject, _distortionMaterial, 0.3f);
        _heatWake.startColor = Color.white;
        _heatWake.endColor = Color.white;
        _heatWake.enabled = _distortionMaterial != null;

        _light = gameObject.AddComponent<Light>();
        _light.type = LightType.Point;
        _light.shadows = LightShadows.None;

        if (assets != null && assets.OrbGlow != null)
        {
            _coreGlow = Instantiate(assets.OrbGlow, transform).transform;
            _coreGlow.localPosition = Vector3.zero;
        }

        RegenerateArcs();
        RegenerateSpikes();
        StartGlitch(0.05f);
    }

    private static LineRenderer CreateLine(Transform parent, string name, Material material, int points, bool loop)
    {
        GameObject lineObject = new GameObject(name);
        lineObject.transform.SetParent(parent, false);
        LineRenderer line = lineObject.AddComponent<LineRenderer>();
        line.useWorldSpace = false;
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

    private static TrailRenderer CreateTrail(GameObject owner, Material material, float time)
    {
        TrailRenderer trail = owner.AddComponent<TrailRenderer>();
        trail.sharedMaterial = material;
        trail.time = time;
        trail.minVertexDistance = 0.1f;
        trail.widthCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 0f));
        trail.shadowCastingMode = ShadowCastingMode.Off;
        trail.enabled = material != null;
        return trail;
    }

    private static void SetCircle(LineRenderer line, float radius)
    {
        for (int i = 0; i < RingPoints; i++)
        {
            float angle = i * Mathf.PI * 2f / RingPoints;
            line.SetPosition(i, new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * radius);
        }
    }

    private static void SetColor(LineRenderer line, Color start, Color end)
    {
        line.startColor = start;
        line.endColor = end;
    }

    private static Vector3 RingAxis(int index)
    {
        switch (index)
        {
            case 0: return Vector3.forward;
            case 1: return Vector3.right;
            default: return Vector3.up;
        }
    }

    #endregion
}
