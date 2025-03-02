using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SlideSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject[] _slides;
    private GameObject _currentSlide;
    private int _index;

    public void ChangeSlide()
    {
        if(_currentSlide != null)
            _currentSlide.SetActive(false);
        _currentSlide = _slides[_index];
        _currentSlide.SetActive(true);
        Debug.Log(_index);
        _index += 1;
        if (_index >= _slides.Length)
            _index = 0;
    }
}