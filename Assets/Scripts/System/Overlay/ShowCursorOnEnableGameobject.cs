using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCursorOnEnableGameobject : MonoBehaviour
{
    private void OnEnable()
    {
        GameCursor.Instance.Show();
    }

    private void OnDisable()
    {
        GameCursor.Instance.ToPrevState();
    }
}