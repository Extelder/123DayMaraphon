using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTakeUp : MonoBehaviour
{
    [field: SerializeField] public bool TakeUpped { get; private set; }

    // Стрелять и юзать абилку можно сразу после свапа — анимация доставания остаётся только визуалом.
    private void OnEnable()
    {
        TakeUpped = true;
    }

    public void TakeUpAnimationEnd()
    {
        TakeUpped = true;
    }
}