using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayStopParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particle;

    public void StopParticle()
    {
        _particle.Stop();
    }

    public void Play()
    {
        _particle.Play();
    }
}