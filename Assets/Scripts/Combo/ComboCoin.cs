using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

// "Монета" с авто-рикошетом: выскакивает из точки, крутится и сразу бьёт лучом в цель.
// Без цели просто подлетает и падает. Объекты собираются в коде и переиспользуются.
public class ComboCoin : MonoBehaviour
{
    private static readonly Color CoinColor = new Color(1f, 0.8f, 0.3f);
    private static readonly Color BeamColor = new Color(1f, 0.9f, 0.5f);

    private const float PopTime = 0.14f;
    private const float BeamTime = 0.16f;
    private const float FallTime = 0.6f;
    private const float BeamWidth = 0.14f;
    private const float SpinSpeed = 1440f;
    private const float HitStopTime = 0.04f;
    private const float HitStopCooldown = 0.15f;

    private static readonly List<ComboCoin> Coins = new List<ComboCoin>();
    private static float _lastHitStopTime = -1f;

    private Transform _disc;
    private TrailRenderer _trail;
    private LineRenderer _beam;
    private Light _glint;
    private Vector3 _velocity;

    public static void Launch(Vector3 origin, Collider target, IWeaponVisitor visitor, float damage, Ghost source,
        Pools pools)
    {
        ComboCoin coin = GetCoin(pools);
        coin.transform.position = origin;
        coin.gameObject.SetActive(true);
        coin._trail.Clear();
        coin.StartCoroutine(coin.Fly(target, visitor, damage, source, pools));
    }

    public static bool IsAlive(IWeaponVisitor visitor)
    {
        if (visitor is UnitHitBox hitBox)
            return hitBox != null && hitBox.CanBeHit;
        return visitor is Object unityObject && unityObject != null;
    }

    private static ComboCoin GetCoin(Pools pools)
    {
        Coins.RemoveAll(coin => coin == null);
        foreach (ComboCoin coin in Coins)
        {
            if (!coin.gameObject.activeSelf)
                return coin;
        }

        ComboCoin created = Create(pools);
        Coins.Add(created);
        return created;
    }

    private static ComboCoin Create(Pools pools)
    {
        GameObject root = new GameObject("ComboCoin");
        root.SetActive(false);
        ComboCoin coin = root.AddComponent<ComboCoin>();

        ComboAssets assets = ComboAssets.Instance;
        GameObject disc;
        if (assets != null && assets.CoinMesh != null && assets.CoinMaterial != null)
        {
            disc = new GameObject("Disc", typeof(MeshFilter), typeof(MeshRenderer));
            disc.GetComponent<MeshFilter>().sharedMesh = assets.CoinMesh;
            disc.GetComponent<MeshRenderer>().sharedMaterial = assets.CoinMaterial;
            Vector3 size = assets.CoinMesh.bounds.size;
            float largest = Mathf.Max(size.x, Mathf.Max(size.y, size.z));
            disc.transform.localScale = Vector3.one * (assets.CoinDiameter / Mathf.Max(largest, 0.0001f));
        }
        else
        {
            disc = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            Destroy(disc.GetComponent<Collider>());
            disc.GetComponent<MeshRenderer>().material.color = CoinColor;
            disc.transform.localScale = new Vector3(0.35f, 0.025f, 0.35f);
        }

        disc.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
        disc.transform.SetParent(root.transform, false);
        coin._disc = disc.transform;

        Material beamMaterial = ComboFx.GetBeamMaterial(pools);

        coin._trail = root.AddComponent<TrailRenderer>();
        coin._trail.sharedMaterial = beamMaterial;
        coin._trail.time = 0.12f;
        coin._trail.widthMultiplier = 0.08f;
        coin._trail.minVertexDistance = 0.05f;
        coin._trail.startColor = BeamColor;
        coin._trail.endColor = new Color(BeamColor.r, BeamColor.g, BeamColor.b, 0f);
        coin._trail.shadowCastingMode = ShadowCastingMode.Off;

        GameObject beamObject = new GameObject("Beam");
        beamObject.transform.SetParent(root.transform, false);
        coin._beam = beamObject.AddComponent<LineRenderer>();
        coin._beam.sharedMaterial = beamMaterial;
        coin._beam.positionCount = 2;
        coin._beam.useWorldSpace = true;
        coin._beam.startColor = Color.white;
        coin._beam.endColor = BeamColor;
        coin._beam.shadowCastingMode = ShadowCastingMode.Off;
        coin._beam.enabled = false;

        GameObject glintObject = new GameObject("Glint");
        glintObject.transform.SetParent(root.transform, false);
        coin._glint = glintObject.AddComponent<Light>();
        coin._glint.type = LightType.Point;
        coin._glint.shadows = LightShadows.None;
        coin._glint.color = CoinColor;
        coin._glint.range = 5f;
        coin._glint.intensity = 0f;

        return coin;
    }

    private IEnumerator Fly(Collider target, IWeaponVisitor visitor, float damage, Ghost source, Pools pools)
    {
        _beam.enabled = false;
        _glint.intensity = 2f;
        _velocity = Vector3.up * Random.Range(6f, 8f) + Random.insideUnitSphere * 1.5f;

        // Время масштабированное: в хитстоп монета замирает вместе с миром.
        float time = 0f;
        while (time < PopTime)
        {
            time += Time.deltaTime;
            Move(Time.deltaTime);
            yield return null;
        }

        if (target != null && target.gameObject.activeInHierarchy && visitor != null && IsAlive(visitor))
        {
            Vector3 from = transform.position;
            Vector3 to = target.bounds.center;
            visitor.Visit(source, damage);
            Ricochet(from, pools);
            yield return BeamRoutine(from, to);
        }
        else
        {
            time = 0f;
            while (time < FallTime)
            {
                time += Time.deltaTime;
                Move(Time.deltaTime);
                yield return null;
            }
        }

        gameObject.SetActive(false);
    }

    private IEnumerator BeamRoutine(Vector3 from, Vector3 to)
    {
        _beam.SetPosition(0, from);
        _beam.SetPosition(1, to);
        _beam.enabled = true;

        // Луч гаснет в реальном времени — даже во время хитстопа рикошета.
        float time = 0f;
        while (time < BeamTime)
        {
            time += Time.unscaledDeltaTime;
            float fade = 1f - Mathf.Clamp01(time / BeamTime);
            _beam.widthMultiplier = BeamWidth * fade;
            _glint.intensity = 8f * fade;
            _disc.Rotate(Vector3.right, SpinSpeed * 2f * Time.unscaledDeltaTime, Space.Self);
            yield return null;
        }

        _beam.enabled = false;
    }

    private void Ricochet(Vector3 position, Pools pools)
    {
        _glint.intensity = 8f;

        ComboAssets assets = ComboAssets.Instance;
        if (assets != null)
            ComboFx.SpawnFx(assets.CoinBurstPrefab, position, 0.5f);

        if (pools != null)
            pools.RiflePool.GetFreeElement(position, Quaternion.identity);

        if (PlayerTime.Instance != null && Time.unscaledTime - _lastHitStopTime > HitStopCooldown)
        {
            _lastHitStopTime = Time.unscaledTime;
            PlayerTime.Instance.TimeStop(HitStopTime);
        }
    }

    private void Move(float deltaTime)
    {
        _velocity += Physics.gravity * deltaTime;
        transform.position += _velocity * deltaTime;
        _disc.Rotate(Vector3.right, SpinSpeed * deltaTime, Space.Self);
    }
}
