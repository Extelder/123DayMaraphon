using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour
{
    [SerializeField] private string _link;
    public void OpenLink()
    {
        Application.OpenURL(_link);
    }
}
