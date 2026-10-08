using UnityEngine;

// Вспышка света, которая гаснет и удаляет себя. Время масштабированное: в хитстоп вспышка держится.
public class FadingLight : MonoBehaviour
{
    private Light _light;
    private float _peak;
    private float _duration;
    private float _time;

    public void Play(Light light, float peak, float duration)
    {
        _light = light;
        _peak = peak;
        _duration = Mathf.Max(duration, 0.01f);
        _time = 0f;
        _light.intensity = peak;
    }

    private void Update()
    {
        if (_light == null)
            return;

        _time += Time.deltaTime;
        float t = 1f - Mathf.Clamp01(_time / _duration);
        _light.intensity = _peak * t * t;
        if (_time >= _duration)
            Destroy(gameObject);
    }
}
