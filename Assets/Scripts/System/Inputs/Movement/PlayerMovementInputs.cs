using System;
using UnityEngine;

public class PlayerMovementInputs : MonoBehaviour
{
    [field: SerializeField] public KeyCode DashKeyCode { get; private set; }
    [field: SerializeField] public KeyCode DasDownhKeyCode { get; private set; }
    [field: SerializeField] public KeyCode JumpKeyCode { get; private set; }
    public float MovementHorizontal { get; private set; }
    public float MovementVertical { get; private set; }
    public event Action DashPressedDown;
    public event Action DashDownwardsPressedDown;
    public event Action JumpPressedDown;
    public bool IsMoving { get; set; }

    public void GetMovingInputs()
    {
        MovementHorizontal = Input.GetAxisRaw("Horizontal");
        MovementVertical = Input.GetAxisRaw("Vertical");
        IsMoving = true;
    }

    private void Update()
    {

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
