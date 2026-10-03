using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormsOpen : MonoBehaviour
{
    private void OnApplicationQuit()
    {
        if (PlayerPrefs.GetInt("FormsOpen", 0) == 0)
        {
            Application.OpenURL("https://forms.gle/Kg1z2B2VR3QbiPNt8");
            PlayerPrefs.SetInt("FormsOpen", 1);
        }
    }
}