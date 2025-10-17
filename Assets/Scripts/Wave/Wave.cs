using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.AI.Navigation;
using UnityEngine;
using Random = UnityEngine.Random;

public class Wave : MonoBehaviour
{
    [SerializeField] private NavMeshSurface _surface;
    [SerializeField] private GameObject[] _levelVariants;
    [field: SerializeField] public Vector3 DefaultSpawnRadius { get; private set; }

    [SerializeField] private int _defaultTime;
    [SerializeField] private int _timeAddible;
    [SerializeField] private int _timeCap;

    private int _currentTime;

    [SerializeField] private GameObject _currentLevel;

    public int Current { get; private set; }

    public float CostMultiplier { get; private set; } = 1f;

    public event Action<int> Started;
    public event Action<int> PreStarted;
    public event Action<int> Ended;
    public event Action<long> TimerCounted;


    private CompositeDisposable _disposable = new CompositeDisposable();

    public static Wave Instance { get; private set; }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            return;
        }
    }

    private void Start()
    {
        _currentTime = _defaultTime;
        StartWave();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(
            transform.position + new Vector3(-DefaultSpawnRadius.x, DefaultSpawnRadius.y, -DefaultSpawnRadius.z),
            transform.position + new Vector3(DefaultSpawnRadius.x, DefaultSpawnRadius.y, DefaultSpawnRadius.z));

        Gizmos.DrawLine(
            transform.position + new Vector3(DefaultSpawnRadius.x, DefaultSpawnRadius.y, DefaultSpawnRadius.z),
            transform.position +
            new Vector3(-DefaultSpawnRadius.x * -1, DefaultSpawnRadius.y, -DefaultSpawnRadius.z * -1));
    }

    public void StartWave()
    {
        _currentLevel.SetActive(false);
        _currentLevel = _levelVariants[Random.Range(0, _levelVariants.Length)];
        _currentLevel.SetActive(true);
        _surface.BuildNavMesh();

        Transform player = PlayerCharacter.Instance.Transform;

        Vector3 raycastStart = player.position + Vector3.up * 100f;

        RaycastHit hit;
        if (Physics.Raycast(raycastStart, Vector3.down, out hit, 100f, LayerMask.GetMask("Default")))
        {
            PlayerCharacter.Instance.Transform.position = hit.point + new Vector3(0, 1, 0);
        }

        Current++;
        PreStarted?.Invoke(Current);
        Started?.Invoke(Current);

        if (Current > 1)
            CostMultiplier *= 1.2f;
        _currentTime = _currentTime + _timeAddible * Current;
        if (_currentTime > _timeCap)
        {
            _currentTime = _timeCap;
        }

        TimerCounted?.Invoke(_currentTime);
        int timePassed = 0;

        Observable.Interval(TimeSpan.FromSeconds(1)).TakeWhile(time => time <= _currentTime)
            .Subscribe(time =>
            {
                timePassed++;
                long delta = (int) _currentTime - timePassed;

                TimerCounted?.Invoke(delta);

                if (timePassed >= _currentTime)
                {
                    StopWave();
                }
            }).AddTo(_disposable);
    }

    public void StopWave()
    {
        _disposable?.Clear();
        Ended?.Invoke(Current);
        Invoke(nameof(StartWave), 1f);
    }

    private void OnDisable()
    {
        _disposable?.Clear();
    }
}