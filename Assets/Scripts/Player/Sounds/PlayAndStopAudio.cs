using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAndStopAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;
    public void Play()
    {
        _audio.Play();
    }

    public void Stop()
    {
        _audio.Stop();
    }
}
