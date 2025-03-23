using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImageColorChangable : MonoBehaviour
{
    [SerializeField] private Slider _sliderR;
    [SerializeField] private Slider _sliderG;
    [SerializeField] private Slider _sliderB;
    [SerializeField] private TextMeshProUGUI _rText;
    [SerializeField] private TextMeshProUGUI _gText;
    [SerializeField] private TextMeshProUGUI _bText;
    [SerializeField] private Image _imageToRepaint;
    private Image _image;
    private void Awake()
    {
        _image = GetComponent<Image>();
        _image.color = new Color(PlayerPrefs.GetFloat("R", 1f), PlayerPrefs.GetFloat("G", 0.5f),
            PlayerPrefs.GetFloat("B", 0.5f));
        _sliderR.value = PlayerPrefs.GetFloat("R");
        _sliderG.value = PlayerPrefs.GetFloat("G");
        _sliderB.value = PlayerPrefs.GetFloat("B");
        _imageToRepaint.color = _image.color;
    }

    public void ChangeR(float value)
    {
        _image.color = new Color(value, _image.color.g, _image.color.b);
        PlayerPrefs.SetFloat("R", value);
        _rText.text = value.ToString("0.00");
    }

    public void ChangeG(float value)
    {
        _image.color = new Color(_image.color.r, value, _image.color.b);
        PlayerPrefs.SetFloat("G", value);
        _gText.text = value.ToString("0.00");
    }

    public void ChangeB(float value)
    {
        _image.color = new Color(_image.color.r, _image.color.g, value);
        PlayerPrefs.SetFloat("B", value);
        _bText.text = value.ToString("0.00");
    }
}
