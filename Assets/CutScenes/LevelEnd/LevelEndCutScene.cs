using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class LevelEndCutScene : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource _impulse;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private AudioSource _audio;

    [SerializeField] private Animator _animator;

    public void StartCutScene()
    {
        _animator.SetTrigger("Finish");
    }

    public void PlaySoundShakeAndParticle()
    {
        _impulse.GenerateImpulse();
        _particle.Play();
        _audio.Play();
    }
}