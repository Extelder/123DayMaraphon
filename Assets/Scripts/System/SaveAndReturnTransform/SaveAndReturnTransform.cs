using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndReturnTransform : MonoBehaviour
{
    private Vector3 _position;
    private Vector3 _eulerAngles;
    private Vector3 _scale;

    private void Awake()
    {
        _position = transform.localPosition;
        _eulerAngles = transform.localEulerAngles;
        _scale = transform.localScale;
    }

    private void OnEnable()
    {
        transform.localPosition = _position;
        transform.localEulerAngles = _eulerAngles;
        transform.localScale = _scale;
    }

    private void OnDisable()
    {
        transform.localPosition = _position;
        transform.localEulerAngles = _eulerAngles;
        transform.localScale = _scale;
    }
}