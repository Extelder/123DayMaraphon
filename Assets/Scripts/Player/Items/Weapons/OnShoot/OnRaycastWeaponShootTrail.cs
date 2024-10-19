using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRaycastWeaponShootTrail : MonoBehaviour
{
    [SerializeField] private RaycastWeaponShoot _weaponShoot;
    [SerializeField] private Transform _nullHitSafeTrailTargetPoint;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Pool _trailPool;

    private void OnEnable()
    {
        _weaponShoot.ShootPerformedWithRaycastHit += ShootPerformed;
    }

    private void OnDisable()
    {
        _weaponShoot.ShootPerformedWithRaycastHit -= ShootPerformed;
    }

    private void ShootPerformed(RaycastHit? hit)
    {
        Vector3 point;
        if (hit == null)
        {
            point = _nullHitSafeTrailTargetPoint.position + _weaponShoot.CurrentShootOffset;
        }
        else
        {
            point = (Vector3) hit?.point;
        }


        TrailRenderer trailRenderer = _trailPool.GetFreeElement(_spawnPoint.position, Quaternion.identity)
            .GetComponent<TrailRenderer>();
        StartCoroutine(SpawnTrail(trailRenderer, point));
    }

    private IEnumerator SpawnTrail(TrailRenderer trailRenderer, Vector3 point)
    {
        float time = 0;
        Vector3 startPosition = trailRenderer.transform.position;

        while (time < 1)
        {
            trailRenderer.transform.position = Vector3.Lerp(startPosition, point, time);
            time += Time.deltaTime / trailRenderer.time;
            yield return null;
        }

        trailRenderer.transform.position = point;
    }
}