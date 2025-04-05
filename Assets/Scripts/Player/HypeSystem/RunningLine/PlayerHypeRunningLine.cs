using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHypeRunningLine : MonoBehaviour
{
    [SerializeField] private float _xWhenSpawn;
    [SerializeField] private float _comboCayoutTime;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Pool _linePool;

    private List<HypeType> _hypes = new List<HypeType>();

    private HypeRunningLine _currentLine;

    public string CurrentHypeName { get; private set; }

    private bool _canSpawn = true;

    private int _currentMultiplier = 1;

    private bool _hypeWaiting;

    private Coroutine _cayoutCoroutine;

    private void Start()
    {
        StartCoroutine(Running());
    }

    public void AddHype(HypeType hypeType)
    {
        if (_hypeWaiting)
        {
            StopCoroutine(_cayoutCoroutine);
            _currentMultiplier++;
            _cayoutCoroutine = StartCoroutine(WaitForCombo(hypeType));
            return;
        }

        _cayoutCoroutine = StartCoroutine(WaitForCombo(hypeType));
    }

    private IEnumerator WaitForCombo(HypeType hypeType)
    {
        _hypeWaiting = true;
        yield return new WaitForSeconds(_comboCayoutTime);
        _hypeWaiting = false;
        if (_currentMultiplier >= 10)
        {
            _currentMultiplier = 1;
            _hypes.Add(HypeType.GODLIKE);
            yield break;
        }

        if (_currentMultiplier >= 6)
        {
            _currentMultiplier = 1;
            _hypes.Add(HypeType.MEGAKILL);
            yield break;
        }

        switch (_currentMultiplier)
        {
            case 1:
                _hypes.Add(hypeType);
                _currentMultiplier = 1;
                yield break;
            case 2:
                _hypes.Add(HypeType.DoubleKill);
                _currentMultiplier = 1;
                yield break;
            case 3:
                _hypes.Add(HypeType.TripleKill);
                _currentMultiplier = 1;
                yield break;
            case 4:
                _hypes.Add(HypeType.QuadroKill);
                _currentMultiplier = 1;
                yield break;
            case 5:
                _hypes.Add(HypeType.RAMPAGE);
                _currentMultiplier = 1;
                yield break;
        }
    }

    private IEnumerator Running()
    {
        while (true)
        {
            yield return new WaitUntil(() => (_hypes.Count > 0));

            if (_canSpawn)
            {
                _canSpawn = false;
                SpawnLine();
            }
            else
            {
                yield return new WaitUntil(() =>
                    _currentLine.transform.localPosition.x <= _xWhenSpawn);
                SpawnLine();
            }
        }
    }

    private void SpawnLine()
    {
        HypeType currentHypeType = _hypes[_hypes.Count - 1];
        CurrentHypeName = currentHypeType.ToString();
        _currentLine = _linePool
            .GetFreeElement(_spawnPoint.position, Quaternion.Euler(0, 0, 0))
            .GetComponent<HypeRunningLine>();
        _currentLine.transform.localEulerAngles = new Vector3(0, 0, 0);
        _currentLine.transform.localPosition = new Vector3(_currentLine.transform.localPosition.x,
            _currentLine.transform.localPosition.y, 0);
        _hypes.Remove(_hypes[_hypes.Count - 1]);
    }
}