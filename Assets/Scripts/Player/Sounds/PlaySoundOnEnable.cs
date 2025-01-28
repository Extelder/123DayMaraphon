using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnEnable : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;

    private void OnEnable()
    {
        _audio.Play();
    }
}