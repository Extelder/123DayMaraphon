using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PUSShardsSound : MonoBehaviour
{
    [SerializeField] private IceProjectile _iceProjectile;

    [SerializeField] private AudioClip[] _shardClips;
    [SerializeField] private AudioSource _source;

    private void OnEnable()
    {
        _iceProjectile.IceBumped += OnIceBumped;
    }

    private void OnDisable()
    {
        _iceProjectile.IceBumped -= OnIceBumped;
    }

    private void OnIceBumped()
    {
        _source.clip = _shardClips[Random.Range(0, _shardClips.Length)];
        _source.Play();
    }
}
