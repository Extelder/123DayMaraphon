using System;
using UnityEngine;

public class PlayerMovementInputs : MonoBehaviour
{
    public KeyCode DashKeyCode => KeyBindings.Get(GameAction.Dash);
    public KeyCode DasDownhKeyCode => KeyBindings.Get(GameAction.DashDown);
    public KeyCode JumpKeyCode => KeyBindings.Get(GameAction.Jump);
    public float MovementHorizontal { get; private set; }
    public float MovementVertical { get; private set; }
    public event Action DashPressedDown;
    public event Action DashDownwardsPressedDown;
    public event Action JumpPressedDown;
    public bool IsMoving { get; set; }

    private Settings _settings;

    public void GetMovingInputs()
    {
        MovementHorizontal = KeyBindings.GetAxis(GameAction.MoveRight, GameAction.MoveLeft);
        MovementVertical = KeyBindings.GetAxis(GameAction.MoveForward, GameAction.MoveBack);
        IsMoving = true;
    }

    private void Start()
    {
        _settings = Settings.Instance;
    }

    private void Update()
    {
        if (_settings.Open)
            return;
        if (Input.GetKeyDown(DashKeyCode))
        {
            DashPressedDown?.Invoke();
        }

        if (Input.GetKeyDown(DasDownhKeyCode))
        {
            DashDownwardsPressedDown?.Invoke();
        }

        if (Input.GetKeyDown(JumpKeyCode))
        {
            JumpPressedDown?.Invoke();
        }
    }
}