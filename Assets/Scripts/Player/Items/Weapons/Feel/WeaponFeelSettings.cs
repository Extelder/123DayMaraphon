using System;
using UnityEngine;

public enum WeaponFeelKind
{
    Rifle,
    Shotgun,
    RPG,
    Railgun,
    Katana
}

[Serializable]
public class WeaponFeelProfile
{
    [Header("Viewmodel recoil")]
    [Tooltip("x — случайный сдвиг вбок, y — вверх, z — назад (метры)")]
    public Vector3 KickPosition;

    [Tooltip("x — задир ствола, y — случайный рыск, z — случайный крен (градусы)")]
    public Vector3 KickRotation;

    [Tooltip("Какая доля отдачи применяется мгновенно, в кадр выстрела")]
    [Range(0f, 1f)] public float Snap = 0.6f;

    public float ReturnSpeed = 10f;
    public float Stiffness = 180f;
    public float Damping = 14f;

    [Header("Camera")]
    [Tooltip("x — задир, y — случайный рыск, z — случайный крен (градусы)")]
    public Vector3 CameraKick;

    public float CameraReturnSpeed = 10f;
    public float FovPunch;

    [Header("Muzzle light")]
    public Color LightColor = Color.white;
    public float LightIntensity;
    public float LightRange = 8f;
    public float LightDuration = 0.08f;

    [Header("Alt fire")]
    public float AbilityMultiplier = 1.4f;
}

// Чтобы крутить значения в инспекторе (в том числе в Play Mode), создай ассет через
// Create > KLITTER > Weapon Feel Settings и положи его в Assets/Resources с именем WeaponFeelSettings.
[CreateAssetMenu(fileName = "WeaponFeelSettings", menuName = "KLITTER/Weapon Feel Settings")]
public class WeaponFeelSettings : ScriptableObject
{
    public const string ResourcesPath = "WeaponFeelSettings";

    [Range(0f, 2f)] public float Intensity = 1f;

    [Header("Camera spring")]
    public float CameraStiffness = 240f;
    public float CameraDamping = 20f;

    [Header("Hit confirm")]
    public Vector3 HitCameraKick = new Vector3(0.2f, 0.3f, 0.6f);
    public float HitFovPunch = -0.9f;

    public WeaponFeelProfile Rifle = new WeaponFeelProfile
    {
        KickPosition = new Vector3(0.004f, 0.006f, 0.035f),
        KickRotation = new Vector3(2.2f, 0.9f, 1.6f),
        Snap = 0.45f,
        ReturnSpeed = 16f,
        Stiffness = 280f,
        Damping = 20f,
        CameraKick = new Vector3(0.45f, 0.18f, 0.3f),
        CameraReturnSpeed = 16f,
        FovPunch = 0.7f,
        LightColor = new Color(1f, 0.82f, 0.45f),
        LightIntensity = 3.5f,
        LightRange = 7f,
        LightDuration = 0.05f,
    };

    public WeaponFeelProfile Shotgun = new WeaponFeelProfile
    {
        KickPosition = new Vector3(0.012f, 0.03f, 0.13f),
        KickRotation = new Vector3(11f, 2.5f, 6f),
        Snap = 0.65f,
        ReturnSpeed = 8f,
        Stiffness = 170f,
        Damping = 12f,
        CameraKick = new Vector3(2.2f, 0.6f, 1.4f),
        CameraReturnSpeed = 9f,
        FovPunch = 4f,
        LightColor = new Color(1f, 0.6f, 0.25f),
        LightIntensity = 6f,
        LightRange = 10f,
        LightDuration = 0.08f,
    };

    public WeaponFeelProfile RPG = new WeaponFeelProfile
    {
        KickPosition = new Vector3(0.01f, 0.04f, 0.18f),
        KickRotation = new Vector3(13f, 2f, 4f),
        Snap = 0.7f,
        ReturnSpeed = 6f,
        Stiffness = 140f,
        Damping = 11f,
        CameraKick = new Vector3(3f, 0.8f, 1.8f),
        CameraReturnSpeed = 7f,
        FovPunch = 6f,
        LightColor = new Color(1f, 0.45f, 0.18f),
        LightIntensity = 8f,
        LightRange = 12f,
        LightDuration = 0.12f,
    };

    public WeaponFeelProfile Railgun = new WeaponFeelProfile
    {
        KickPosition = new Vector3(0.008f, 0.02f, 0.15f),
        KickRotation = new Vector3(7f, 2f, 8f),
        Snap = 0.75f,
        ReturnSpeed = 7f,
        Stiffness = 160f,
        Damping = 10f,
        CameraKick = new Vector3(2.4f, 0.4f, 2.5f),
        CameraReturnSpeed = 8f,
        FovPunch = 5f,
        LightColor = new Color(0.35f, 0.85f, 1f),
        LightIntensity = 9f,
        LightRange = 12f,
        LightDuration = 0.12f,
    };

    public WeaponFeelProfile Katana = new WeaponFeelProfile
    {
        KickPosition = new Vector3(0.04f, 0.01f, -0.03f),
        KickRotation = new Vector3(-3f, 6f, 14f),
        Snap = 0.5f,
        ReturnSpeed = 10f,
        Stiffness = 200f,
        Damping = 15f,
        CameraKick = new Vector3(0.4f, 1.2f, 2.2f),
        CameraReturnSpeed = 12f,
        FovPunch = 1.5f,
        LightIntensity = 0f,
    };

    public WeaponFeelProfile Get(WeaponFeelKind kind)
    {
        switch (kind)
        {
            case WeaponFeelKind.Shotgun: return Shotgun;
            case WeaponFeelKind.RPG: return RPG;
            case WeaponFeelKind.Railgun: return Railgun;
            case WeaponFeelKind.Katana: return Katana;
            default: return Rifle;
        }
    }

    public static WeaponFeelSettings Load()
    {
        WeaponFeelSettings settings = Resources.Load<WeaponFeelSettings>(ResourcesPath);
        return settings != null ? settings : CreateInstance<WeaponFeelSettings>();
    }
}
