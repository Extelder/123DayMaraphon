using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAndStopAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;
    public void PlayAudio()
    {
        _audio.Play();
    }

    public void StopAudio()
    {
        _audio.Stop();
    }
}
