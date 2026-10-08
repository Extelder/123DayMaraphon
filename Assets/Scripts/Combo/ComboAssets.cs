using System;
using UnityEngine;

// Ассеты для комбо и эффектов, которые код берёт без правок префабов: лежит в Resources/ComboAssets.
[CreateAssetMenu(fileName = "ComboAssets", menuName = "KLITTER/Combo Assets")]
public class ComboAssets : ScriptableObject
{
    private const string ResourcesPath = "ComboAssets";

    private static ComboAssets _instance;

    [Serializable]
    public struct LevelColor
    {
        public string SceneName;
        public Color Color;
    }

    [Header("Coin")]
    public Mesh CoinMesh;
    public Material CoinMaterial;
    public GameObject CoinBurstPrefab;
    public float CoinDiameter = 0.4f;

    [Header("Shot tracers")]
    public Material RailGlowMaterial;

    [Header("Explosions (случайный вариант из списка)")]
    public GameObject[] RocketExplosions;
    public GameObject[] BigExplosionExtras;
    public GameObject[] Flashes;
    public GameObject[] Shockwaves;
    public GameObject[] Sparks;
    public GameObject[] BoomTexts;
    public GameObject[] GhostTexts;
    public GameObject[] BallExplosions;
    public GameObject[] EnemyExplosions;
    public GameObject[] GhostExplosions;
    public GameObject NukeExplosion;
    public GameObject BallMaxExplosion;
    public GameObject GroundCrack;

    [Header("Level colors (пусто — цвет берётся из тумана/неба уровня)")]
    public LevelColor[] LevelColors;

    [Header("Rift cross (волна РПГ)")]
    public Material RiftDistortionMaterial;
    public Material RiftGlowMaterial;
    public GameObject RiftSparks;

    [Header("Energy orb (шар рейлгана)")]
    public GameObject OrbGlow;

    public static ComboAssets Instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<ComboAssets>(ResourcesPath);
            return _instance;
        }
    }

    public static GameObject Pick(GameObject[] variants)
    {
        if (variants == null || variants.Length == 0)
            return null;
        return variants[UnityEngine.Random.Range(0, variants.Length)];
    }
}
