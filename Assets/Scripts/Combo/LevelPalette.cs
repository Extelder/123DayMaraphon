using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

// Цвет уровня для эффектов: ручной оверрайд из ComboAssets, иначе самый насыщенный из тумана,
// тинта скайбокса, окружения и солнца. Считается один раз на сцену.
public static class LevelPalette
{
    private static readonly Color Fallback = new Color(1f, 0.45f, 0.15f);
    private static readonly string[] SkyColorProperties = { "_Tint", "_SkyTint", "_Color", "_TopColor", "_SkyColor" };

    private static int _sceneHandle = int.MinValue;
    private static Color _color = Fallback;

    public static Color Color
    {
        get
        {
            Scene scene = SceneManager.GetActiveScene();
            if (scene.handle != _sceneHandle)
            {
                _sceneHandle = scene.handle;
                _color = Detect(scene.name);
            }

            return _color;
        }
    }

    public static float Hue
    {
        get
        {
            UnityEngine.Color.RGBToHSV(Color, out float hue, out _, out _);
            return hue;
        }
    }

    private static Color Detect(string sceneName)
    {
        ComboAssets assets = ComboAssets.Instance;
        if (assets != null && assets.LevelColors != null)
        {
            foreach (ComboAssets.LevelColor levelColor in assets.LevelColors)
            {
                if (levelColor.SceneName == sceneName)
                    return Vivid(levelColor.Color);
            }
        }

        Color best = Fallback;
        float bestScore = 0.12f;

        if (RenderSettings.fog)
            Consider(RenderSettings.fogColor, 1.1f, ref best, ref bestScore);

        Material sky = RenderSettings.skybox;
        if (sky != null)
        {
            foreach (string property in SkyColorProperties)
            {
                if (sky.HasProperty(property))
                    Consider(sky.GetColor(property), 1f, ref best, ref bestScore);
            }
        }

        Vector3[] directions = { Vector3.up, Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
        Color[] samples = new Color[directions.Length];
        SphericalHarmonicsL2 ambientProbe = RenderSettings.ambientProbe;
        ambientProbe.Evaluate(directions, samples);
        Color ambient = UnityEngine.Color.black;
        foreach (Color sample in samples)
            ambient += sample / samples.Length;
        Consider(ambient, 0.9f, ref best, ref bestScore);

        if (RenderSettings.sun != null)
            Consider(RenderSettings.sun.color, 0.8f, ref best, ref bestScore);

        return Vivid(best);
    }

    private static void Consider(Color candidate, float weight, ref Color best, ref float bestScore)
    {
        UnityEngine.Color.RGBToHSV(candidate, out _, out float saturation, out float value);
        if (value < 0.05f)
            return;

        float score = saturation * weight;
        if (score > bestScore)
        {
            bestScore = score;
            best = candidate;
        }
    }

    private static Color Vivid(Color color)
    {
        UnityEngine.Color.RGBToHSV(color, out float hue, out float saturation, out _);
        return UnityEngine.Color.HSVToRGB(hue, Mathf.Max(saturation, 0.75f), 1f);
    }
}
