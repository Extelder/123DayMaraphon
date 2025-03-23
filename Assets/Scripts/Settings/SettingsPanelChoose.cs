using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanelChoose : MonoBehaviour
{
    [SerializeField] private GameObject _currentPanel;
    public void ChoosePanel(GameObject panel)
    {
        _currentPanel?.SetActive(false);
        _currentPanel = panel;
        panel.SetActive(true);
    }
}
