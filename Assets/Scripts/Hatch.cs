using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatch : MonoBehaviour
{
    private void Start()
    {
        if( !PlayerPrefs.HasKey("CurrentScene")){
            gameObject.SetActive(false);
        }
    }

}
