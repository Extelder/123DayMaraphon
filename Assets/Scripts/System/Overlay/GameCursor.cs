using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CursorState
{
    public CursorLockMode CursorLockMode;
    public bool Visible;
}

public class GameCursor : MonoBehaviour
{
    public static GameCursor Instance { get; private set; }

    public CursorState PrevCursorState;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            return;
        }

        Debug.LogError("There`s one more GameCursor");
        Debug.Log(gameObject);
        Debug.Break();
    }

    private void OnEnable()
    {
        Hide();
    }

    public void ToPrevState()
    {
        Cursor.visible = PrevCursorState.Visible;
        Cursor.lockState = PrevCursorState.CursorLockMode;
    }

    public void Show()
    {
        PrevCursorState.CursorLockMode = CursorLockMode.Locked;
        PrevCursorState.Visible = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Hide()
    {
        PrevCursorState.CursorLockMode = CursorLockMode.None;
        PrevCursorState.Visible = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}