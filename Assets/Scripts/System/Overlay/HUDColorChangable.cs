using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDColorChangable : MonoBehaviour
{
    
    [SerializeField] private Slider _sliderR;
    [SerializeField] private Slider _sliderG;
    [SerializeField] private Slider _sliderB;
    [SerializeField] private TextMeshProUGUI _rText;
    [SerializeField] private TextMeshProUGUI _gText;
    [SerializeField] private TextMeshProUGUI _bText;
    [SerializeField] private string _r;
    [SerializeField] private string _g;
    [SerializeField] private string _b;
    [SerializeField] private float _defaultR;
    [SerializeField] private float _defaultG;
    [SerializeField] private float _defaultB;
    private Image _image;
    private void Awake()
    {
        _image = GetComponent<Image>();
        _sliderR.value = PlayerPrefs.GetFloat(_r, _defaultR);
        _sliderG.value = PlayerPrefs.GetFloat(_g, _defaultG);
        _sliderB.value = PlayerPrefs.GetFloat(_b, _defaultB);
        _rText.text = _sliderR.value.ToString("0.00");
        _gText.text = _sliderG.value.ToString("0.00");
        _bText.text = _sliderB.value.ToString("0.00");
        _image.material.color = new Color(PlayerPrefs.GetFloat(_r, _defaultR), PlayerPrefs.GetFloat(_g, _defaultG),
            PlayerPrefs.GetFloat(_b, _defaultB));
    }

    public void ChangeR(float value)
    {
        _image.material.color = new Color(value, _image.material.color.g, _image.material.color.b);
        PlayerPrefs.SetFloat(_r, value);
        _rText.text = value.ToString("0.00");
    }

    public void ChangeG(float value)
    {
        _image.material.color = new Color(_image.material.color.r, value, _image.material.color.b);
        PlayerPrefs.SetFloat(_g, value);
        _gText.text = value.ToString("0.00");
    }

    public void ChangeB(float value)
    {
        _image.material.color = new Color(_image.material.color.r, _image.material.color.g, value);
        PlayerPrefs.SetFloat(_b, value);
        _bText.text = value.ToString("0.00");
    }
}
