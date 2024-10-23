using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticlesOnEnable : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particle;

    private void OnEnable()
    {
        _particle.Play();
    }
}