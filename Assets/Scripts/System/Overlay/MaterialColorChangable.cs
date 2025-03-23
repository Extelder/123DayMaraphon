using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialColorChangable : MonoBehaviour
{
    private Renderer _renderer;
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material.color = new Color((PlayerPrefs.GetFloat("R")), PlayerPrefs.GetFloat("G"),
            PlayerPrefs.GetFloat("B"));
    }

    public void ChangeR(float value)
    {
        _renderer.material.color = new Color(value, _renderer.material.color.g, _renderer.material.color.b);
        PlayerPrefs.SetFloat("R", value);
    }

    public void ChangeG(float value)
    {
        _renderer.material.color = new Color(_renderer.material.color.r, value, _renderer.material.color.b);
        PlayerPrefs.SetFloat("G", value);
    }

    public void ChangeB(float value)
    {
        _renderer.material.color = new Color(_renderer.material.color.r, _renderer.material.color.g, value);
        PlayerPrefs.SetFloat("B", value);
    }
}
