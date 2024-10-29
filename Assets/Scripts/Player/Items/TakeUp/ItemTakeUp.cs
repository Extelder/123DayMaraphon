using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTakeUp : MonoBehaviour
{
    [field: SerializeField] public bool TakeUpped { get; private set; }

    private void OnEnable()
    {
        TakeUpped = false;
    }

    public void TakeUpAnimationEnd()
    {
        TakeUpped = true;
    }
}