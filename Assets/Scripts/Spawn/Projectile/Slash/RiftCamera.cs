using UnityEngine;
using UnityEngine.Rendering.Universal;

// Искажению разлома нужны opaque- и depth-текстуры камеры, а в URP-ассетах они выключены ради
// производительности. Включаем их на основной камере, только пока в мире есть хоть один разлом.
public static class RiftCamera
{
    private static int _users;
    private static UniversalAdditionalCameraData _cameraData;
    private static CameraOverrideOption _originalColor;
    private static CameraOverrideOption _originalDepth;

    public static void Acquire()
    {
        _users++;
        if (_users > 1 && _cameraData != null)
            return;

        Camera camera = Camera.main;
        if (camera == null)
            return;

        _cameraData = camera.GetUniversalAdditionalCameraData();
        _originalColor = _cameraData.requiresColorOption;
        _originalDepth = _cameraData.requiresDepthOption;
        _cameraData.requiresColorOption = CameraOverrideOption.On;
        _cameraData.requiresDepthOption = CameraOverrideOption.On;
    }

    public static void Release()
    {
        _users = Mathf.Max(0, _users - 1);
        if (_users > 0 || _cameraData == null)
            return;

        _cameraData.requiresColorOption = _originalColor;
        _cameraData.requiresDepthOption = _originalDepth;
        _cameraData = null;
    }
}
