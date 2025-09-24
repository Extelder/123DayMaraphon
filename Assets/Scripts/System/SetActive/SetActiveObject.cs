using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveObject : MonoBehaviour
{
    [SerializeField] private GameObject _object;
    
    public void SetActive(bool show)
    {
        _object.SetActive(show);
    }
}
