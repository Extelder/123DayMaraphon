using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomlyPlayAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _randomSoundsSource;
    [SerializeField] private AudioClip[] _randomSounds;
    [SerializeField] private float _cooldown;
    
    
    public IEnumerator RandomizeSounds()
    {
        _randomSoundsSource.clip = _randomSounds[Random.Range(0, _randomSounds.Length)];
        _randomSoundsSource.Play();
        yield return new WaitForSeconds(_cooldown);
    }
}
