using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class RandomRotatingBox : MonoBehaviour
{
    [SerializeField] private Ease _rotationEase;

    [SerializeField] private float _rotateTime;

    [SerializeField] private float _startDelay = 5;
    [SerializeField] private float _delay;

    private Tween _currentTween;

    private NavMeshSurface[] _surfaces;
    private NavMeshAgent[] _agents;


    private void Start()
    {
        _surfaces = GetComponentsInChildren<NavMeshSurface>();
        _agents = GetComponentsInChildren<NavMeshAgent>(true);

        StartCoroutine(RotatingWithDelay());
    }

    private IEnumerator RotatingWithDelay()
    {
        yield return new WaitForSeconds(_startDelay);

        while (true)
        {
            Vector3 targetEluerAngles = transform.localEulerAngles;

            switch (Random.Range(1, 5))
            {
                case 1:
                    targetEluerAngles += new Vector3(90, 0, 0);
                    break;
                case 2:
                    targetEluerAngles += new Vector3(90, 0, 0);
                    break;
                case 3:
                    targetEluerAngles += new Vector3(0, 0, 90);
                    break;
                case 4:
                    targetEluerAngles += new Vector3(0, 0, 90);
                    break;
                case 5:
                    targetEluerAngles += new Vector3(90, 0, 0);
                    break;
            }


            for (int i = 0; i < _agents.Length; i++)
            {
                if (_agents[i] != null)
                    _agents[i].enabled = false;
            }

            for (int i = 0; i < _surfaces.Length; i++)
            {
                _surfaces[i].enabled = false;
            }

            _currentTween = transform.DOLocalRotate(targetEluerAngles, _rotateTime).SetEase(_rotationEase);
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => !_currentTween.active);
            for (int i = 0; i < _surfaces.Length; i++)
            {
                _surfaces[i].enabled = true;
            }

            for (int i = 0; i < _agents.Length; i++)
            {
                if (_agents[i] != null)
                    _agents[i].enabled = true;
            }

            yield return new WaitForSeconds(_delay);
        }
    }

    private void OnDisable()
    {
        _currentTween?.Kill();
    }
}