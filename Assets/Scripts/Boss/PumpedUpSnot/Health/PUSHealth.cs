using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUSHealth : Health
{
    [SerializeField] private Renderer _bossModel;
    [SerializeField] private Renderer[] _eyes;

    private Material _material;

    private Material[] _eyesMaterials = new Material[2];

    private void Awake()
    {
        _material = _bossModel.material;

        for (int i = 0; i < _eyes.Length; i++)
        {
            _eyesMaterials[i] = _eyes[i].material;
        }
    }

    public override void ChangeHealthValue(float value)
    {
        base.ChangeHealthValue(value);
        float hue = value / 1000;

        _material.SetColor("_AlbedoColor", Color.HSVToRGB(0, 0, hue - 0.1f, true));
        for (int i = 0; i < _eyesMaterials.Length; i++)
        {
            _eyesMaterials[i].SetColor("_AlbedoColor", Color.HSVToRGB(0, (1 - hue) + 0.2f, 1, true));
        }
    }


    public override void Death()
    {
    }
}