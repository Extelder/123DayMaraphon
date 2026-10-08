using System.Collections.Generic;
using UnityEngine;

// Перекраска партиклов в оттенок уровня: насыщенные цвета сдвигаются к нужному hue, белое ядро
// и серый дым остаются как есть. Исходные цвета запоминаются, чтобы при переиспользовании не копить сдвиг.
public static class ParticleTint
{
    private struct Original
    {
        public ParticleSystem.MinMaxGradient Start;
        public ParticleSystem.MinMaxGradient OverLifetime;
    }

    private static readonly Dictionary<int, Original> Originals = new Dictionary<int, Original>();

    public static void Apply(GameObject root, float hue, float strength)
    {
        if (root == null || strength <= 0f)
            return;

        if (Originals.Count > 2048)
            Originals.Clear();

        foreach (ParticleSystem system in root.GetComponentsInChildren<ParticleSystem>(true))
        {
            int id = system.GetInstanceID();
            ParticleSystem.MainModule main = system.main;
            ParticleSystem.ColorOverLifetimeModule overLifetime = system.colorOverLifetime;

            if (!Originals.TryGetValue(id, out Original original))
            {
                original = new Original { Start = main.startColor, OverLifetime = overLifetime.color };
                Originals.Add(id, original);
            }

            main.startColor = Recolor(original.Start, hue, strength);
            if (overLifetime.enabled)
                overLifetime.color = Recolor(original.OverLifetime, hue, strength);
        }
    }

    public static Color Recolor(Color color, float hue, float strength)
    {
        Color.RGBToHSV(color, out float h, out float s, out float v);
        if (s < 0.1f)
            return color;

        float shifted = Mathf.Repeat(Mathf.LerpAngle(h * 360f, hue * 360f, strength) / 360f, 1f);
        Color result = Color.HSVToRGB(shifted, Mathf.Lerp(s, Mathf.Max(s, 0.8f), strength), v, true);
        result.a = color.a;
        return result;
    }

    private static ParticleSystem.MinMaxGradient Recolor(ParticleSystem.MinMaxGradient source, float hue,
        float strength)
    {
        switch (source.mode)
        {
            case ParticleSystemGradientMode.Color:
                return new ParticleSystem.MinMaxGradient(Recolor(source.color, hue, strength));

            case ParticleSystemGradientMode.TwoColors:
                return new ParticleSystem.MinMaxGradient(Recolor(source.colorMin, hue, strength),
                    Recolor(source.colorMax, hue, strength));

            case ParticleSystemGradientMode.Gradient:
                return new ParticleSystem.MinMaxGradient(Recolor(source.gradient, hue, strength));

            case ParticleSystemGradientMode.TwoGradients:
                return new ParticleSystem.MinMaxGradient(Recolor(source.gradientMin, hue, strength),
                    Recolor(source.gradientMax, hue, strength));

            case ParticleSystemGradientMode.RandomColor:
                ParticleSystem.MinMaxGradient random =
                    new ParticleSystem.MinMaxGradient(Recolor(source.gradient, hue, strength));
                random.mode = ParticleSystemGradientMode.RandomColor;
                return random;

            default:
                return source;
        }
    }

    private static Gradient Recolor(Gradient source, float hue, float strength)
    {
        if (source == null)
            return null;

        GradientColorKey[] colorKeys = source.colorKeys;
        for (int i = 0; i < colorKeys.Length; i++)
            colorKeys[i].color = Recolor(colorKeys[i].color, hue, strength);

        Gradient result = new Gradient { mode = source.mode };
        result.SetKeys(colorKeys, source.alphaKeys);
        return result;
    }
}
