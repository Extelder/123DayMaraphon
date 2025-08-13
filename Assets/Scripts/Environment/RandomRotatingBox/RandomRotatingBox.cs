using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomRotatingBox : MonoBehaviour
{
    [SerializeField] private float _rotateTime;

    [SerializeField] private float _delay;

    private Tween _currentTween;

    private void Start()
    {
        StartCoroutine(RotatingWithDelay());
    }

    private IEnumerator RotatingWithDelay()
    {
        while (true)
        {
            Vector3 targetEluerAngles = transform.localEulerAngles;

            switch (Random.Range(1, 3))
            {
                case 1:
                    targetEluerAngles += new Vector3(0, 90, 0);
                    break;
                case 2:
                    targetEluerAngles += new Vector3(90, 0, 0);
                    break;
                case 3:
                    targetEluerAngles += new Vector3(0, 0, 90);
                    break;
            }


            _currentTween = transform.DOLocalRotate(targetEluerAngles, _rotateTime);
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => !_currentTween.active);
            yield return new WaitForSeconds(_delay);
        }
    }
}