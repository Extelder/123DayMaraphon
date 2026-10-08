using System.Collections.Generic;
using CartoonFX;
using UnityEngine;

public static class ComboFx
{
    private static readonly Dictionary<GameObject, List<GameObject>> Instances =
        new Dictionary<GameObject, List<GameObject>>();

    private static Material _beamMaterial;

    static ComboFx()
    {
        // Камеру трясёт WeaponFeel через Cinemachine; собственный шейк CFXR с ним бы дрался.
        CFXR_Effect.GlobalDisableCameraShake = true;
    }

    public static void FlashLight(Vector3 position, Color color, float intensity, float range, float duration)
    {
        GameObject lightObject = new GameObject("ComboFlashLight");
        lightObject.transform.position = position;

        Light light = lightObject.AddComponent<Light>();
        light.type = LightType.Point;
        light.shadows = LightShadows.None;
        light.color = color;
        light.range = range;
        lightObject.AddComponent<FadingLight>().Play(light, intensity, duration);
    }

    // Партиклы без пула в проекте: переигрываем отыгравшие экземпляры, новые создаём по необходимости.
    // CFXR-эффекты удаляют себя сами — такие просто пересоздаются.
    public static GameObject SpawnFx(GameObject prefab, Vector3 position, Quaternion rotation, float scale)
    {
        if (prefab == null)
            return null;

        if (!Instances.TryGetValue(prefab, out List<GameObject> instances))
        {
            instances = new List<GameObject>();
            Instances.Add(prefab, instances);
        }

        instances.RemoveAll(instance => instance == null);
        GameObject fx = instances.Find(instance => !IsPlaying(instance));
        if (fx == null)
        {
            fx = Object.Instantiate(prefab);
            instances.Add(fx);
        }

        fx.transform.SetPositionAndRotation(position, rotation);
        fx.transform.localScale = Vector3.one * scale;
        fx.SetActive(true);

        foreach (ParticleSystem system in fx.GetComponentsInChildren<ParticleSystem>(true))
        {
            // Hierarchy — иначе дочерние системы с Local-масштабом не растут вместе со взрывом;
            // без зацикливания — эффект одноразовый и потом переиспользуется.
            ParticleSystem.MainModule main = system.main;
            main.scalingMode = ParticleSystemScalingMode.Hierarchy;
            main.loop = false;
        }

        foreach (ParticleSystem system in fx.GetComponentsInChildren<ParticleSystem>(true))
        {
            if (system.transform.parent == null || system.transform.parent.GetComponentInParent<ParticleSystem>() == null)
            {
                system.Clear(true);
                system.Play(true);
            }
        }

        return fx;
    }

    public static GameObject SpawnFx(GameObject prefab, Vector3 position, float scale)
    {
        return SpawnFx(prefab, position, Quaternion.identity, scale);
    }

    // Материал трейла рейлгана: лучи комбо выглядят как родные выстрелы и точно есть в билде.
    public static Material GetBeamMaterial(Pools pools)
    {
        if (_beamMaterial != null)
            return _beamMaterial;

        TrailRenderer trail = pools != null ? pools.RailgunTrailPool.GetComponentInChildren<TrailRenderer>(true) : null;
        _beamMaterial = trail != null ? trail.sharedMaterial : new Material(Shader.Find("Sprites/Default"));
        return _beamMaterial;
    }

    private static bool IsPlaying(GameObject instance)
    {
        if (!instance.activeInHierarchy)
            return false;

        foreach (ParticleSystem system in instance.GetComponentsInChildren<ParticleSystem>())
        {
            if (system.IsAlive(false))
                return true;
        }

        return false;
    }
}
