using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Удар взрыва по экрану: хроматика, "выпуклость" линзы и вспышка экспозиции в цвет уровня.
// Глобальный Volume создаётся в рантайме и гаснет сам.
public class ExplosionPostFx : MonoBehaviour
{
    private const float FadeSpeed = 2.2f;

    private static ExplosionPostFx _instance;

    private Volume _volume;
    private ColorAdjustments _colorAdjustments;
    private LensDistortion _lens;
    private float _weight;

    // lensIntensity < 0 — экран "выпирает" (взрыв), > 0 — "засасывает" (сингулярность).
    public static void Punch(float strength, Color tint, float lensIntensity = -0.35f)
    {
        if (strength <= 0.01f)
            return;

        if (_instance == null)
            _instance = Create();

        _instance._lens.intensity.value = lensIntensity;
        _instance._weight = Mathf.Clamp01(Mathf.Max(_instance._weight, strength));
        _instance._colorAdjustments.colorFilter.value = Color.Lerp(Color.white, tint, 0.35f);
        _instance._volume.weight = _instance._weight;
    }

    private static ExplosionPostFx Create()
    {
        GameObject root = new GameObject("ExplosionPostFx");

        // Слой как у существующих Volume — иначе маска камеры может его не видеть.
        Volume existing = FindObjectOfType<Volume>();
        if (existing != null)
            root.layer = existing.gameObject.layer;

        ExplosionPostFx postFx = root.AddComponent<ExplosionPostFx>();
        postFx._volume = root.AddComponent<Volume>();
        postFx._volume.isGlobal = true;
        postFx._volume.priority = 200f;
        postFx._volume.weight = 0f;

        VolumeProfile profile = ScriptableObject.CreateInstance<VolumeProfile>();
        ChromaticAberration chromatic = profile.Add<ChromaticAberration>(true);
        chromatic.intensity.Override(1f);
        postFx._lens = profile.Add<LensDistortion>(true);
        postFx._lens.intensity.Override(-0.35f);
        postFx._colorAdjustments = profile.Add<ColorAdjustments>(true);
        postFx._colorAdjustments.postExposure.Override(0.9f);
        postFx._colorAdjustments.colorFilter.Override(Color.white);
        postFx._volume.sharedProfile = profile;

        return postFx;
    }

    private void Update()
    {
        if (_weight <= 0f)
            return;

        // Реальное время: во время хитстопа удар по экрану всё равно отыгрывает.
        _weight = Mathf.MoveTowards(_weight, 0f, FadeSpeed * Time.unscaledDeltaTime);
        _volume.weight = _weight * _weight;
    }
}
