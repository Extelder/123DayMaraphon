using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ButtonChangeScale : MonoBehaviour
{
    [SerializeField] private float _scaleEndValue;
    [SerializeField] private float _scaleStartValue;
    [SerializeField] private float _duration;

    private Tween _tween;
    
    public void OnButtonEnter(GameObject gameObject)
    {
        _tween = gameObject.transform.DOScale(_scaleEndValue, _duration);
    }
    
    public void OnButtonExit(GameObject gameObject)
    {
        _tween = gameObject.transform.DOScale(_scaleStartValue, _duration);
        _tween.Kill();
    }
}
