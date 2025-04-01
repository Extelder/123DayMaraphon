using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class KunitanaUltimate : MonoBehaviour
{
    [field: SerializeField] public float Damage { get; private set; }

    [SerializeField] private PlayerHypeSystem _playerHypeSystem;
    [SerializeField] private float _cooldown;
    
    [SerializeField] private KeyCode _ultimateKeyCode;

    [SerializeField] private GameObject _weapons;
    [SerializeField] private GameObject _kunitanas;

    [SerializeField] private AudioMixer _generalMixer;

    [SerializeField] private Settings _settings;

    private float _masterVolume;

    private bool _pressed = false;
    private CompositeDisposable _compositeDisposable = new CompositeDisposable();
    public static event Action Ultimated;
    public static event Action UltimateStoped;

    private void OnEnable()
    {
        _playerHypeSystem.HypeChanged += OnHypeChanged;
    }

    private void Start()
    {
        _playerHypeSystem.Add(100000000000);
    }

    private void OnHypeChanged(float value)
    {
        if (value >= 4.9f)
        {
            if (_pressed == false)
            {
                Observable.EveryUpdate().Subscribe(_ =>
                {
                    _pressed = true;
                    if (Input.GetKeyDown(_ultimateKeyCode))
                    {
                        KunitanaAttack();
                        Ultimated?.Invoke();
                        StartCoroutine(StopUltimate());
                    }
                }).AddTo(_compositeDisposable);   
            }
        }
        else
        {
            _compositeDisposable.Clear();
        }
    }

    private IEnumerator StopUltimate()
    {
        yield return new WaitForSeconds(_cooldown);
        ResetKunitanasAttack();
        UltimateStoped?.Invoke();
    }

    private void Update()
    {
        Debug.Log(_masterVolume);
    }

    private void KunitanaAttack()
    {
        _weapons.SetActive(false);
        _kunitanas.SetActive(true);
    }

    private void ResetKunitanasAttack()
    {
        _kunitanas.SetActive(false);
        _weapons.SetActive(true);
    }

    private void OnDisable()
    {
        _playerHypeSystem.HypeChanged -= OnHypeChanged;
        _compositeDisposable.Clear();
    }
}
