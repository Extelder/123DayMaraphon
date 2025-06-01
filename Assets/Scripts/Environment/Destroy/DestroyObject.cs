using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [SerializeField] private GameObject _object;

    public void ObjectDestroy()
    {
        Destroy(_object);
    }
}
