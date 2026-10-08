using System;
using UnityEngine;
using Random = UnityEngine.Random;

// Стиль для всех взрывов поверх родных партиклов снарядов. Каждый взрыв собирается из нескольких
// слоёв со случайными вариантами (огненный шар, вспышка, ударная волна, искры, дым, трещины, комикс-бумы)
// и перекрашивается в цвет уровня. Камеру трясёт WeaponFeel, экран бьёт ExplosionPostFx.
public static class ExplosionFx
{
    public enum Kind
    {
        Rocket,
        RocketBig,
        Ball,
        BallMax,
        Nuke,
        Enemy,
        Ghost,
        Singularity
    }

    private struct Recipe
    {
        public float ReferenceRadius;
        public float MinScale;
        public float MaxScale;
        public float Tint;
        public int Booms;
        public float BoomChance;
        public float BoomInterval;
        public float ExtraChance;
        public float LightIntensity;
        public float LightDuration;
        public float LightRange;
        public float ScreenPunch;
        public bool SkipFlash;
    }

    public static event Action<Vector3, float, Kind> Exploded;

    public static void Play(Vector3 position, float radius, Kind kind)
    {
        radius = Mathf.Max(radius, 0.5f);
        Recipe recipe = GetRecipe(kind);
        Color levelColor = LevelPalette.Color;
        float hue = LevelPalette.Hue;
        float scale = Mathf.Clamp(radius / recipe.ReferenceRadius, recipe.MinScale, recipe.MaxScale);

        ComboAssets assets = ComboAssets.Instance;
        if (assets != null)
            SpawnLayers(assets, kind, recipe, position, radius, scale, hue);

        Color lightColor = kind == Kind.Ball ? Color.Lerp(levelColor, new Color(0.4f, 0.85f, 1f), 0.5f) : levelColor;
        float lightRange = radius * (recipe.LightRange > 0f ? recipe.LightRange : 3f);
        ComboFx.FlashLight(position, lightColor, recipe.LightIntensity, lightRange, recipe.LightDuration);

        PunchScreen(position, radius, recipe.ScreenPunch, levelColor);
        Exploded?.Invoke(position, radius, kind);
    }

    private static void SpawnLayers(ComboAssets assets, Kind kind, Recipe recipe, Vector3 position, float radius,
        float scale, float hue)
    {
        switch (kind)
        {
            case Kind.Nuke:
                Spawn(assets.NukeExplosion, position, scale, hue, recipe.Tint * 0.5f);
                Spawn(ComboAssets.Pick(assets.BallExplosions), position, scale * 0.8f, hue, recipe.Tint);
                break;
            case Kind.Singularity:
                Spawn(ComboAssets.Pick(assets.BallExplosions), position, scale * 1.4f, hue, recipe.Tint);
                Spawn(ComboAssets.Pick(assets.RocketExplosions), position, scale, hue, recipe.Tint);
                break;
            case Kind.BallMax:
                Spawn(assets.BallMaxExplosion, position, scale, hue, recipe.Tint * 0.5f);
                Spawn(ComboAssets.Pick(assets.BallExplosions), position, scale * 1.3f, hue, recipe.Tint);
                break;
            case Kind.Ball:
                Spawn(ComboAssets.Pick(assets.BallExplosions), position, scale, hue, recipe.Tint);
                break;
            case Kind.Enemy:
                Spawn(ComboAssets.Pick(assets.EnemyExplosions), position, scale, hue, recipe.Tint);
                break;
            case Kind.Ghost:
                Spawn(ComboAssets.Pick(assets.GhostExplosions), position, scale, hue, recipe.Tint);
                break;
            default:
                Spawn(ComboAssets.Pick(assets.RocketExplosions), position, scale, hue, recipe.Tint);
                break;
        }

        if (kind == Kind.Enemy)
            return;

        // Вспышка-спрайт даёт сильный засвет — у самых больших взрывов её нет, остаётся форма.
        if (!recipe.SkipFlash)
            Spawn(ComboAssets.Pick(assets.Flashes), position, scale, hue, recipe.Tint);

        if (Random.value < recipe.ExtraChance)
            Spawn(ComboAssets.Pick(assets.Sparks), position, scale, hue, recipe.Tint);

        bool big = kind == Kind.RocketBig || kind == Kind.BallMax || kind == Kind.Nuke || kind == Kind.Singularity;
        if (big || Random.value < recipe.ExtraChance)
            Spawn(ComboAssets.Pick(assets.BigExplosionExtras), position, scale, hue, recipe.Tint);

        if (TryGround(position, radius, out RaycastHit ground))
        {
            Quaternion onGround = Quaternion.FromToRotation(Vector3.up, ground.normal);
            Spawn(ComboAssets.Pick(assets.Shockwaves), ground.point + ground.normal * 0.1f, onGround, scale, hue,
                recipe.Tint);
            if (big || kind == Kind.Rocket)
                Spawn(assets.GroundCrack, ground.point + ground.normal * 0.05f, onGround, scale * 0.8f, hue,
                    recipe.Tint);
        }

        SpawnBooms(assets, kind, recipe, position, radius, hue);
    }

    // Комикс-бумы: у больших взрывов — серия с задержками вокруг центра, ближе к камере.
    private static void SpawnBooms(ComboAssets assets, Kind kind, Recipe recipe, Vector3 position, float radius,
        float hue)
    {
        if (Random.value > recipe.BoomChance)
            return;

        GameObject[] texts = kind == Kind.Ghost ? assets.GhostTexts : assets.BoomTexts;
        Camera camera = Camera.main;
        Vector3 towardCamera = camera != null ? (camera.transform.position - position).normalized : Vector3.zero;
        float textScale = Mathf.Clamp(radius / 6f, 0.8f, 4f);

        for (int i = 0; i < recipe.Booms; i++)
        {
            Vector3 offset = i == 0
                ? Vector3.up * radius * 0.25f
                : Random.insideUnitSphere * radius * 0.6f + Vector3.up * radius * 0.3f;
            Vector3 boomPosition = position + offset + towardCamera * radius * 0.3f;
            float boomScale = textScale * (i == 0 ? 1.2f : Random.Range(0.6f, 1f));
            GameObject prefab = ComboAssets.Pick(texts);

            FxRunner.Delay(i * recipe.BoomInterval, () =>
                Spawn(prefab, boomPosition, Quaternion.Euler(0f, 0f, Random.Range(-18f, 18f)), boomScale, hue,
                    recipe.Tint));
        }
    }

    private static void PunchScreen(Vector3 position, float radius, float punch, Color tint)
    {
        Camera camera = Camera.main;
        if (camera == null || punch <= 0f)
            return;

        float distance = Vector3.Distance(camera.transform.position, position);
        float falloff = 1f - Mathf.Clamp01(distance / (radius * 5f + 15f));
        ExplosionPostFx.Punch(punch * falloff, tint);
    }

    private static void Spawn(GameObject prefab, Vector3 position, float scale, float hue, float tint)
    {
        Spawn(prefab, position, Quaternion.identity, scale, hue, tint);
    }

    private static void Spawn(GameObject prefab, Vector3 position, Quaternion rotation, float scale, float hue,
        float tint)
    {
        GameObject fx = ComboFx.SpawnFx(prefab, position, rotation, scale);
        ParticleTint.Apply(fx, hue, tint);
    }

    private static bool TryGround(Vector3 position, float radius, out RaycastHit hit)
    {
        return Physics.Raycast(position + Vector3.up * 0.5f, Vector3.down, out hit, radius + 1f,
            Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
    }

    private static Recipe GetRecipe(Kind kind)
    {
        switch (kind)
        {
            case Kind.RocketBig:
                return new Recipe
                {
                    ReferenceRadius = 3f, MinScale = 1.3f, MaxScale = 6f, Tint = 0.85f,
                    Booms = 2, BoomChance = 1f, BoomInterval = 0.12f, ExtraChance = 1f,
                    LightIntensity = 24f, LightDuration = 0.5f, ScreenPunch = 0.8f,
                };
            case Kind.Ball:
                return new Recipe
                {
                    ReferenceRadius = 4f, MinScale = 0.9f, MaxScale = 5f, Tint = 0.55f,
                    Booms = 1, BoomChance = 0.5f, BoomInterval = 0.1f, ExtraChance = 0.8f,
                    LightIntensity = 16f, LightDuration = 0.4f, ScreenPunch = 0.5f,
                };
            case Kind.BallMax:
                return new Recipe
                {
                    ReferenceRadius = 8f, MinScale = 1.2f, MaxScale = 4.5f, Tint = 0.6f,
                    Booms = 3, BoomChance = 1f, BoomInterval = 0.14f, ExtraChance = 1f,
                    LightIntensity = 18f, LightDuration = 0.6f, LightRange = 1.8f, ScreenPunch = 0.7f,
                    SkipFlash = true,
                };
            case Kind.Nuke:
                // Меньше и без засвета: гриб в ~2 раза меньше, свет короче и слабее, экран бьёт мягче.
                return new Recipe
                {
                    ReferenceRadius = 22f, MinScale = 0.8f, MaxScale = 3.2f, Tint = 0.6f,
                    Booms = 3, BoomChance = 1f, BoomInterval = 0.16f, ExtraChance = 1f,
                    LightIntensity = 14f, LightDuration = 0.6f, LightRange = 1.2f, ScreenPunch = 0.5f,
                    SkipFlash = true,
                };
            case Kind.Singularity:
                return new Recipe
                {
                    ReferenceRadius = 4f, MinScale = 1.5f, MaxScale = 5f, Tint = 0.85f,
                    Booms = 3, BoomChance = 1f, BoomInterval = 0.12f, ExtraChance = 1f,
                    LightIntensity = 30f, LightDuration = 0.8f, ScreenPunch = 1f,
                };
            case Kind.Enemy:
                return new Recipe
                {
                    ReferenceRadius = 5f, MinScale = 0.6f, MaxScale = 2.5f, Tint = 0.35f,
                    Booms = 0, BoomChance = 0f, ExtraChance = 0f,
                    LightIntensity = 6f, LightDuration = 0.2f, ScreenPunch = 0.15f,
                };
            case Kind.Ghost:
                return new Recipe
                {
                    ReferenceRadius = 4f, MinScale = 0.8f, MaxScale = 3f, Tint = 0.7f,
                    Booms = 1, BoomChance = 0.35f, BoomInterval = 0.1f, ExtraChance = 0.5f,
                    LightIntensity = 9f, LightDuration = 0.3f, ScreenPunch = 0.25f,
                };
            default:
                return new Recipe
                {
                    ReferenceRadius = 4f, MinScale = 0.9f, MaxScale = 4f, Tint = 0.85f,
                    Booms = 1, BoomChance = 0.6f, BoomInterval = 0.1f, ExtraChance = 0.6f,
                    LightIntensity = 14f, LightDuration = 0.35f, ScreenPunch = 0.45f,
                };
        }
    }
}
