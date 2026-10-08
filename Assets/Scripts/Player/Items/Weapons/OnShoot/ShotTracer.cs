using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Трассер выстрела: появляется мгновенно на всю длину и держится за дуло, пока гаснет,
// поэтому не отстаёт от ствола при движении. У пуль поверх бежит яркий штрих,
// у рейлгана — толстое ядро, свечение и расширяющаяся спираль.
public class ShotTracer : MonoBehaviour
{
    public enum Style
    {
        Bullet,
        Pellet,
        Rail
    }

    private struct StyleSettings
    {
        public float Width;
        public float Duration;
        public Color Color;
        public float StreakLength;
        public float StreakSpeed;
        public float StreakWidth;
    }

    private static readonly StyleSettings BulletSettings = new StyleSettings
    {
        Width = 0.05f, Duration = 0.09f, Color = new Color(1f, 0.85f, 0.6f),
        StreakLength = 4f, StreakSpeed = 240f, StreakWidth = 0.1f,
    };

    private static readonly StyleSettings PelletSettings = new StyleSettings
    {
        Width = 0.035f, Duration = 0.08f, Color = new Color(1f, 0.7f, 0.45f),
        StreakLength = 2.5f, StreakSpeed = 200f, StreakWidth = 0.07f,
    };

    private static readonly StyleSettings RailSettings = new StyleSettings
    {
        Width = 0.18f, Duration = 0.4f, Color = new Color(0.75f, 0.95f, 1f),
    };

    private static readonly Color RailGlowColor = new Color(0.25f, 0.8f, 1f, 0.6f);
    private const float RailGlowWidth = 0.75f;
    private const float SpiralStep = 0.25f;
    private const float SpiralTurnLength = 1.4f;
    private const int MaxSpiralPoints = 240;

    private static readonly List<ShotTracer> Tracers = new List<ShotTracer>();

    private LineRenderer _core;
    private LineRenderer _streak;
    private LineRenderer _glow;
    private LineRenderer _spiral;

    private Transform _muzzle;
    private Vector3 _start;
    private Vector3 _end;
    private Style _style;
    private StyleSettings _settings;
    private float _time;
    private float _lifetime;
    private float _spiralPhase;

    public static void Fire(Transform muzzle, Vector3 end, Style style, Material material, bool hit)
    {
        ShotTracer tracer = GetTracer();
        tracer.Begin(muzzle, end, style, material);

        if (hit && style == Style.Rail)
            ComboFx.FlashLight(end, RailGlowColor, 6f, 6f, 0.2f);
    }

    private static ShotTracer GetTracer()
    {
        Tracers.RemoveAll(tracer => tracer == null);
        foreach (ShotTracer tracer in Tracers)
        {
            if (!tracer.gameObject.activeSelf)
                return tracer;
        }

        GameObject root = new GameObject("ShotTracer");
        ShotTracer created = root.AddComponent<ShotTracer>();
        created._core = CreateLine(root.transform, "Core");
        created._streak = CreateLine(root.transform, "Streak");
        created._glow = CreateLine(root.transform, "Glow");
        created._spiral = CreateLine(root.transform, "Spiral");
        root.SetActive(false);
        Tracers.Add(created);
        return created;
    }

    private static LineRenderer CreateLine(Transform parent, string name)
    {
        GameObject lineObject = new GameObject(name);
        lineObject.transform.SetParent(parent, false);
        LineRenderer line = lineObject.AddComponent<LineRenderer>();
        line.useWorldSpace = true;
        line.positionCount = 2;
        line.numCapVertices = 2;
        line.textureMode = LineTextureMode.Stretch;
        line.shadowCastingMode = ShadowCastingMode.Off;
        line.receiveShadows = false;
        line.enabled = false;
        return line;
    }

    private void Begin(Transform muzzle, Vector3 end, Style style, Material material)
    {
        _muzzle = muzzle;
        _start = muzzle != null ? muzzle.position : end;
        _end = end;
        _style = style;
        _time = 0f;
        _spiralPhase = Random.Range(0f, Mathf.PI * 2f);
        _settings = style == Style.Rail ? RailSettings : style == Style.Pellet ? PelletSettings : BulletSettings;

        float distance = Vector3.Distance(_start, _end);
        _lifetime = _settings.Duration;
        if (_settings.StreakSpeed > 0f)
            _lifetime = Mathf.Max(_lifetime, distance / _settings.StreakSpeed + 0.02f);

        Material glowMaterial = ComboAssets.Instance != null && ComboAssets.Instance.RailGlowMaterial != null
            ? ComboAssets.Instance.RailGlowMaterial
            : material;

        _core.sharedMaterial = material;
        _streak.sharedMaterial = material;
        _glow.sharedMaterial = glowMaterial;
        _spiral.sharedMaterial = glowMaterial;

        _core.enabled = true;
        _streak.enabled = style != Style.Rail;
        _glow.enabled = style == Style.Rail;
        _spiral.enabled = style == Style.Rail;

        gameObject.SetActive(true);
        Refresh();
    }

    private void Update()
    {
        // Масштабированное время: в хитстоп выстрела трассер замирает на весь кадр удара.
        _time += Time.deltaTime;
        if (_time >= _lifetime)
        {
            gameObject.SetActive(false);
            return;
        }

        Refresh();
    }

    private void Refresh()
    {
        if (_muzzle != null && _muzzle.gameObject.activeInHierarchy)
            _start = _muzzle.position;

        float fade = 1f - Mathf.Clamp01(_time / _settings.Duration);
        float easedFade = fade * fade;

        SetLine(_core, _start, _end, _settings.Width * easedFade, _settings.Color, fade);

        if (_style == Style.Rail)
        {
            SetLine(_glow, _start, _end, RailGlowWidth * easedFade, RailGlowColor, fade);
            UpdateSpiral(fade);
            return;
        }

        UpdateStreak();
    }

    private void UpdateStreak()
    {
        Vector3 direction = _end - _start;
        float distance = direction.magnitude;
        if (distance < 0.01f)
        {
            _streak.enabled = false;
            return;
        }

        direction /= distance;
        float head = Mathf.Min(_time * _settings.StreakSpeed, distance);
        float tail = Mathf.Max(0f, head - _settings.StreakLength);
        bool arrived = head >= distance && _time * _settings.StreakSpeed - distance > _settings.StreakLength;
        _streak.enabled = !arrived;
        if (arrived)
            return;

        _streak.SetPosition(0, _start + direction * tail);
        _streak.SetPosition(1, _start + direction * head);
        _streak.widthMultiplier = _settings.StreakWidth;
        _streak.startColor = new Color(1f, 1f, 1f, 0.2f);
        _streak.endColor = Color.white;
    }

    private void UpdateSpiral(float fade)
    {
        Vector3 direction = _end - _start;
        float distance = direction.magnitude;
        if (distance < 0.01f)
        {
            _spiral.enabled = false;
            return;
        }

        direction /= distance;
        Vector3 right = Vector3.Cross(direction, Vector3.up);
        if (right.sqrMagnitude < 0.001f)
            right = Vector3.Cross(direction, Vector3.forward);
        right.Normalize();
        Vector3 up = Vector3.Cross(right, direction);

        int points = Mathf.Clamp(Mathf.CeilToInt(distance / SpiralStep), 8, MaxSpiralPoints);
        float radius = Mathf.Lerp(0.5f, 0.15f, fade);
        _spiral.positionCount = points;
        for (int i = 0; i < points; i++)
        {
            float along = distance * i / (points - 1);
            float angle = _spiralPhase + along / SpiralTurnLength * Mathf.PI * 2f + _time * 6f;
            Vector3 offset = (right * Mathf.Cos(angle) + up * Mathf.Sin(angle)) * radius;
            _spiral.SetPosition(i, _start + direction * along + offset);
        }

        _spiral.widthMultiplier = 0.06f * fade;
        _spiral.startColor = new Color(RailGlowColor.r, RailGlowColor.g, RailGlowColor.b, fade);
        _spiral.endColor = new Color(RailGlowColor.r, RailGlowColor.g, RailGlowColor.b, fade * 0.3f);
    }

    private static void SetLine(LineRenderer line, Vector3 start, Vector3 end, float width, Color color, float alpha)
    {
        line.positionCount = 2;
        line.SetPosition(0, start);
        line.SetPosition(1, end);
        line.widthMultiplier = width;
        line.startColor = new Color(color.r, color.g, color.b, color.a * alpha);
        line.endColor = new Color(color.r, color.g, color.b, color.a * alpha * 0.6f);
    }
}
