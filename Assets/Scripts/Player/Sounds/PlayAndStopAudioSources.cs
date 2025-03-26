using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAndStopAudioSources : MonoBehaviour
{
    [SerializeField] private AudioSource[] _sources;

    public void StartSources()
    {
        for (int i = 0; i < _sources.Length; i++)
        {
            _sources[i].Play();
        }
    }

    public void StopSources()
    {
        for (int i = 0; i < _sources.Length; i++)
        {
            _sources[i].Stop();
        }
    }
}
