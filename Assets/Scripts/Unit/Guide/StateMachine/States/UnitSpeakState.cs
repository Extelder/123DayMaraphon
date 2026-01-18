using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Video;

public class UnitSpeakState : EnemyState
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Transform _unitParent;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private float _printCooldown;
    [SerializeField] private float _maxDistanceDelta;
    [SerializeField] private float _stopDistance;
    private CompositeDisposable _disposable = new CompositeDisposable();
    private string _textToPrint;
    private Transform _targetPoint;
    
    public override void Enter()
    {
        CanChanged = false;
        _text.text = "";
        Animator.Move();
        MoveToTargetPoint();
        _canvas.SetActive(true);
        StartCoroutine(PrintingText());
    }

    private void MoveToTargetPoint()
    {
        Observable.Interval(TimeSpan.FromSeconds(0.02f)).Subscribe(_ =>
        {
            _unitParent.position = Vector3.MoveTowards(_unitParent.position, _targetPoint.position, _maxDistanceDelta);
            if (Vector3.Distance(_unitParent.position, _targetPoint.position) <= _stopDistance) 
            {
                Animator.Attack();
                _disposable.Clear();
            }
        }).AddTo(_disposable);
    }

    private IEnumerator PrintingText()
    {
        for (int i = 0; i < _textToPrint.Length; i++)
        {
            yield return new WaitForSeconds(_printCooldown);
            _text.text += _textToPrint[i];
        }
    }

    public void SetParametrs(string textToPring, Transform targetPoint)
    {
        _textToPrint = textToPring;
        _targetPoint = targetPoint;
    }

    public override void Exit()
    {
        base.Exit();
        _canvas.SetActive(false);
        _text.text = "";
        StopAllCoroutines();
        _disposable.Clear();
    }

    private void OnDisable()
    {
        _disposable.Clear();
    }
}
