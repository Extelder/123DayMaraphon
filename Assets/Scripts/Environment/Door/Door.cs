using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [field: SerializeField] public bool IsOpen { get; private set; }

    [field: SerializeField] public bool IsLock { get; private set; }

    public event Action Locked;
    public event Action UnLocked;
    public event Action Opened;
    public event Action Closed;

    public void TryOpenClose()
    {
        if (IsOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    private void Open()
    {
        IsOpen = true;
        Opened?.Invoke();
    }

    private void Close()
    {
        IsOpen = false;
        Closed?.Invoke();
    }

    public void Lock()
    {
        IsLock = true;
        Locked?.Invoke();
    }

    public void UnLock()
    {
        IsLock = false;
        UnLocked?.Invoke();
    }
}