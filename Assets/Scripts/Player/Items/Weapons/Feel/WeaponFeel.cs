using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

// Ощущение выстрела для всего оружия игрока: пружинная отдача вьюмодели, кик камеры с FOV-панчем,
// вспышка света и хит-панч при попадании. Добавляется на игрока в рантайме и только подписывается
// на события выстрелов — импульсы, хитстоп, volume, партиклы и звуки работают как раньше.
public class WeaponFeel : MonoBehaviour
{
    private const string PivotName = "RecoilPivot";
    private const float MaxDeltaTime = 0.05f;
    private const float SubStep = 1f / 120f;
    private const float MaxAccumulation = 3f;

    private class Spring
    {
        public Vector3 Target;
        public Vector3 Current;
        public Vector3 Velocity;

        public void Kick(Vector3 kick, float snap, Vector3 limit)
        {
            Target = Clamp(Target + kick, limit);
            Current += kick * snap;
        }

        public void Update(float deltaTime, float returnSpeed, float stiffness, float damping)
        {
            Target = Vector3.Lerp(Target, Vector3.zero, 1f - Mathf.Exp(-returnSpeed * deltaTime));
            Vector3 acceleration = (Target - Current) * stiffness - Velocity * damping;
            Velocity += acceleration * deltaTime;
            Current += Velocity * deltaTime;
        }

        private static Vector3 Clamp(Vector3 value, Vector3 limit)
        {
            return new Vector3(
                Mathf.Clamp(value.x, -limit.x, limit.x),
                Mathf.Clamp(value.y, -limit.y, limit.y),
                Mathf.Clamp(value.z, -limit.z, limit.z));
        }
    }

    private class Viewmodel
    {
        public Transform Pivot;
        public Transform Reference;
        public WeaponFeelKind Kind;
        public readonly Spring Position = new Spring();
        public readonly Spring Rotation = new Spring();
    }

    private readonly Dictionary<Transform, Viewmodel> _viewmodels = new Dictionary<Transform, Viewmodel>();
    private readonly List<Action> _unsubscribers = new List<Action>();
    private readonly Spring _cameraRotation = new Spring();
    private readonly Spring _cameraFov = new Spring();

    private WeaponFeelSettings _settings;
    private CinemachineRecoilExtension _cameraRecoil;
    private WeaponFeelKind _cameraKind;
    private int _lastHitFrame = -1;

    private Light _light;
    private float _lightPeak;
    private float _lightDuration;
    private float _lightTimer;

    public static void Install(GameObject player)
    {
        if (player.GetComponent<WeaponFeel>() == null)
            player.AddComponent<WeaponFeel>();
    }

    private static bool CameraKickEnabled => PlayerPrefs.GetInt("CameraShake", 1) == 1;

    private void Start()
    {
        _settings = WeaponFeelSettings.Load();

        CinemachineVirtualCamera virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>(true);
        if (virtualCamera != null)
        {
            _cameraRecoil = virtualCamera.GetComponent<CinemachineRecoilExtension>();
            if (_cameraRecoil == null)
                _cameraRecoil = virtualCamera.gameObject.AddComponent<CinemachineRecoilExtension>();
        }

        CinemachineBrain brain = GetComponentInChildren<CinemachineBrain>(true);
        if (brain != null)
            CreateLight(brain.transform);

        foreach (WeaponShoot shoot in GetComponentsInChildren<WeaponShoot>(true))
        {
            Viewmodel viewmodel = GetViewmodel(shoot.transform);
            WeaponFeelKind kind = KindOf(shoot);
            RaycastWeaponShoot raycast = shoot as RaycastWeaponShoot;
            Action handler = () =>
                Kick(viewmodel, kind, false, raycast != null && raycast.Bursting ? _settings.BurstShotMultiplier : 1f);
            shoot.ShootPerformed += handler;
            _unsubscribers.Add(() => shoot.ShootPerformed -= handler);
        }

        foreach (WeaponAbility ability in GetComponentsInChildren<WeaponAbility>(true))
        {
            Viewmodel viewmodel = GetViewmodel(ability.transform);
            WeaponFeelKind kind = KindOf(ability);
            Action handler = () => Kick(viewmodel, kind, true);
            ability.AbilityPerformed += handler;
            _unsubscribers.Add(() => ability.AbilityPerformed -= handler);
        }

        foreach (KunitanShoot katana in GetComponentsInChildren<KunitanShoot>(true))
        {
            Viewmodel viewmodel = GetViewmodel(katana.transform);
            Action handler = () => Kick(viewmodel, WeaponFeelKind.Katana, false);
            katana.Shooted += handler;
            _unsubscribers.Add(() => katana.Shooted -= handler);
        }

        UnitHitBox.UnitHitted += OnUnitHitted;
        LightningBall.Nuked += OnNuked;
        ExplosionFx.Exploded += OnExploded;
    }

    private void OnDestroy()
    {
        UnitHitBox.UnitHitted -= OnUnitHitted;
        LightningBall.Nuked -= OnNuked;
        ExplosionFx.Exploded -= OnExploded;
        foreach (Action unsubscribe in _unsubscribers)
            unsubscribe();
        _unsubscribers.Clear();
    }

    private void Update()
    {
        if (_settings == null)
            return;

        // Масштабированное время: во время хитстопа и паузы отдача замирает в "ударной" позе.
        float deltaTime = Mathf.Min(Time.deltaTime, MaxDeltaTime);
        if (deltaTime <= 0f)
            return;

        int steps = Mathf.CeilToInt(deltaTime / SubStep);
        float step = deltaTime / steps;
        WeaponFeelProfile cameraProfile = _settings.Get(_cameraKind);

        for (int i = 0; i < steps; i++)
        {
            foreach (Viewmodel viewmodel in _viewmodels.Values)
            {
                WeaponFeelProfile profile = _settings.Get(viewmodel.Kind);
                viewmodel.Position.Update(step, profile.ReturnSpeed, profile.Stiffness, profile.Damping);
                viewmodel.Rotation.Update(step, profile.ReturnSpeed, profile.Stiffness, profile.Damping);
            }

            _cameraRotation.Update(step, cameraProfile.CameraReturnSpeed, _settings.CameraStiffness,
                _settings.CameraDamping);
            _cameraFov.Update(step, cameraProfile.CameraReturnSpeed, _settings.CameraStiffness,
                _settings.CameraDamping);
        }

        foreach (Viewmodel viewmodel in _viewmodels.Values)
            Apply(viewmodel);
        ApplyCamera();
        UpdateLight(deltaTime);
    }

    private void Kick(Viewmodel viewmodel, WeaponFeelKind kind, bool ability, float scale = 1f)
    {
        WeaponFeelProfile profile = _settings.Get(kind);
        float multiplier = _settings.Intensity * scale * (ability ? profile.AbilityMultiplier : 1f);
        if (multiplier <= 0f)
            return;

        if (viewmodel != null)
        {
            viewmodel.Kind = kind;
            Vector3 position = new Vector3(
                profile.KickPosition.x * RandomSign(),
                profile.KickPosition.y,
                -profile.KickPosition.z) * multiplier;
            viewmodel.Position.Kick(position, profile.Snap, Abs(profile.KickPosition) * (MaxAccumulation * multiplier));
            viewmodel.Rotation.Kick(RandomizedKick(profile.KickRotation) * multiplier, profile.Snap,
                Abs(profile.KickRotation) * (MaxAccumulation * multiplier));
            Apply(viewmodel);
        }

        if (CameraKickEnabled)
        {
            _cameraKind = kind;
            _cameraRotation.Kick(RandomizedKick(profile.CameraKick) * multiplier, profile.Snap,
                Abs(profile.CameraKick) * (MaxAccumulation * multiplier));
            _cameraFov.Kick(new Vector3(profile.FovPunch * multiplier, 0, 0), profile.Snap,
                Vector3.one * (Mathf.Abs(profile.FovPunch) * MaxAccumulation * multiplier));
            ApplyCamera();
        }

        Flash(profile, multiplier);
    }

    private void OnUnitHitted()
    {
        // Дробовик бьёт несколькими лучами за кадр — панчим один раз.
        if (_settings == null || _lastHitFrame == Time.frameCount || !CameraKickEnabled)
            return;

        _lastHitFrame = Time.frameCount;
        float intensity = _settings.Intensity;
        _cameraRotation.Kick(RandomizedKick(_settings.HitCameraKick) * intensity, 1f,
            Abs(_settings.HitCameraKick) * (MaxAccumulation * intensity));
        _cameraFov.Kick(new Vector3(_settings.HitFovPunch * intensity, 0, 0), 1f,
            Vector3.one * (Mathf.Abs(_settings.HitFovPunch) * MaxAccumulation * intensity));
        ApplyCamera();
    }

    private void OnExploded(Vector3 position, float radius, ExplosionFx.Kind kind)
    {
        // Nuke трясёт отдельно (OnNuked), чтобы не сложилось дважды.
        if (_settings == null || kind == ExplosionFx.Kind.Nuke || !CameraKickEnabled || _cameraRecoil == null)
            return;

        float distance = Vector3.Distance(_cameraRecoil.transform.position, position);
        float reach = radius * _settings.ExplosionShakeRadii + _settings.ExplosionShakeExtraDistance;
        float falloff = 1f - Mathf.Clamp01(distance / reach);
        if (falloff <= 0f)
            return;

        float strength = falloff * falloff * _settings.Intensity * ExplosionStrength(kind);
        _cameraRotation.Kick(RandomizedKick(_settings.ExplosionCameraKick) * strength, 1f,
            Abs(_settings.ExplosionCameraKick) * (MaxAccumulation * strength));
        _cameraFov.Kick(new Vector3(_settings.ExplosionFovPunch * strength, 0, 0), 1f,
            Vector3.one * (Mathf.Abs(_settings.ExplosionFovPunch) * MaxAccumulation * strength));
        ApplyCamera();
    }

    private static float ExplosionStrength(ExplosionFx.Kind kind)
    {
        switch (kind)
        {
            case ExplosionFx.Kind.RocketBig: return 1.8f;
            case ExplosionFx.Kind.BallMax: return 2.6f;
            case ExplosionFx.Kind.Singularity: return 2.4f;
            case ExplosionFx.Kind.Ball: return 1.2f;
            case ExplosionFx.Kind.Enemy: return 0.5f;
            case ExplosionFx.Kind.Ghost: return 0.7f;
            default: return 1f;
        }
    }

    private void OnNuked(Vector3 position)
    {
        if (_settings == null || !CameraKickEnabled)
            return;

        float intensity = _settings.Intensity;
        _cameraRotation.Kick(RandomizedKick(_settings.NukeCameraKick) * intensity, 1f,
            Abs(_settings.NukeCameraKick) * (MaxAccumulation * intensity));
        _cameraFov.Kick(new Vector3(_settings.NukeFovPunch * intensity, 0, 0), 1f,
            Vector3.one * (Mathf.Abs(_settings.NukeFovPunch) * MaxAccumulation * intensity));
        ApplyCamera();
    }

    // Отдача задаётся в осях камеры, которая рисует вьюмодель, и переводится в локальные оси pivot:
    // у катаны родитель сильно повёрнут, а "назад" и "вверх" должны быть экранными.
    private static void Apply(Viewmodel viewmodel)
    {
        if (viewmodel.Pivot == null || viewmodel.Reference == null)
            return;

        Quaternion toReference = Quaternion.Inverse(viewmodel.Reference.rotation) * viewmodel.Pivot.parent.rotation;
        Quaternion fromReference = Quaternion.Inverse(toReference);
        viewmodel.Pivot.localPosition = fromReference * viewmodel.Position.Current;
        viewmodel.Pivot.localRotation = fromReference * Quaternion.Euler(viewmodel.Rotation.Current) * toReference;
    }

    private void ApplyCamera()
    {
        if (_cameraRecoil == null)
            return;

        _cameraRecoil.KickRotation = _cameraRotation.Current;
        _cameraRecoil.FovOffset = _cameraFov.Current.x;
    }

    private void CreateLight(Transform mainCamera)
    {
        GameObject lightObject = new GameObject("ShotFlashLight");
        lightObject.transform.SetParent(mainCamera, false);
        lightObject.transform.localPosition = new Vector3(0.15f, -0.1f, 1.1f);

        _light = lightObject.AddComponent<Light>();
        _light.type = LightType.Point;
        _light.shadows = LightShadows.None;
        _light.intensity = 0f;
        _light.enabled = false;
    }

    private void Flash(WeaponFeelProfile profile, float multiplier)
    {
        if (_light == null || profile.LightIntensity <= 0f || profile.LightDuration <= 0f)
            return;

        _light.color = profile.LightColor;
        _light.range = profile.LightRange;
        _lightPeak = profile.LightIntensity * multiplier;
        _lightDuration = profile.LightDuration;
        _lightTimer = profile.LightDuration;
        _light.intensity = _lightPeak;
        _light.enabled = true;
    }

    private void UpdateLight(float deltaTime)
    {
        if (_light == null || _lightTimer <= 0f)
            return;

        _lightTimer -= deltaTime;
        float t = Mathf.Clamp01(_lightTimer / _lightDuration);
        _light.intensity = _lightPeak * t * t;
        if (_lightTimer <= 0f)
            _light.enabled = false;
    }

    // Pivot вставляется между держателем оружия (WeaponSway) и TakeUp (Animator),
    // чтобы ни свей, ни анимации не перетирали отдачу.
    private Viewmodel GetViewmodel(Transform source)
    {
        ItemTakeUp takeUp = source.GetComponentInParent<ItemTakeUp>(true);
        if (takeUp == null || takeUp.transform.parent == null)
            return null;

        Transform holder = takeUp.transform.parent;
        Transform pivot;
        if (holder.name == PivotName)
        {
            pivot = holder;
        }
        else
        {
            pivot = new GameObject(PivotName).transform;
            pivot.gameObject.layer = takeUp.gameObject.layer;
            pivot.SetParent(holder, false);
            pivot.SetSiblingIndex(takeUp.transform.GetSiblingIndex());
            takeUp.transform.SetParent(pivot, false);
        }

        if (_viewmodels.TryGetValue(pivot, out Viewmodel viewmodel))
            return viewmodel;

        Camera reference = pivot.GetComponentInParent<Camera>(true);
        viewmodel = new Viewmodel
        {
            Pivot = pivot,
            Reference = reference != null ? reference.transform : pivot.parent,
        };
        _viewmodels.Add(pivot, viewmodel);
        return viewmodel;
    }

    private static WeaponFeelKind KindOf(Component source)
    {
        if (source is ProjectileWeaponShoot || source.GetComponent<RPGAbility>() != null)
            return WeaponFeelKind.RPG;
        if (source.GetComponent<RailgunAbility>() != null || source.GetComponent<OnRailgunShootTrail>() != null)
            return WeaponFeelKind.Railgun;
        if (source.GetComponent<ShotGunAbility>() != null || source.GetComponent<OnWeaponShootAddImpulse>() != null)
            return WeaponFeelKind.Shotgun;
        if (source is WeaponShoot shoot && shoot.Weapon != null && shoot.Weapon.HitsPerShot > 1)
            return WeaponFeelKind.Shotgun;
        return WeaponFeelKind.Rifle;
    }

    private static Vector3 RandomizedKick(Vector3 kick)
    {
        return new Vector3(-kick.x, kick.y * Random.Range(-1f, 1f), kick.z * Random.Range(-1f, 1f));
    }

    private static float RandomSign()
    {
        return Random.value < 0.5f ? -1f : 1f;
    }

    private static Vector3 Abs(Vector3 value)
    {
        return new Vector3(Mathf.Abs(value.x), Mathf.Abs(value.y), Mathf.Abs(value.z));
    }
}
