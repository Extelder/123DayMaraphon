using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerSteppingSound : MonoBehaviour
{
    [SerializeField] private PlayerWalk _walk;
    [SerializeField] private float _stepDelay;
    [SerializeField] private AudioSource _stepSound;
    [SerializeField] private GroundChecker _groundChecker;

    private bool _playingSound;

    private void Update()
    {
        if (_groundChecker.Detected && _walk._moving.Value)
        {
            if (_playingSound)
                return;
            StartCoroutine(Playing());
            _playingSound = true;
            return;
        }

        StopAllCoroutines();
        _playingSound = false;
    }

    private IEnumerator Playing()
    {
        while (true)
        {
            _stepSound.Play();
            yield return new WaitForSeconds(_stepDelay);
        }
    }
}