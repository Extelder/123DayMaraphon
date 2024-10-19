using System;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    public float MovementHorizontal { get; private set; }
    public float MovementVertical { get; private set; }
    [field:SerializeField] public KeyCode DashKeyCode { get; private set; }
    [field:SerializeField] public KeyCode DasDownhKeyCode { get; private set; }
    [field:SerializeField] public KeyCode JumpKeyCode { get; private set; }

    public event Action DashPressed;
    public event Action DashDownPressed;
    public event Action JumpPressed;

    public void GetMovingInputs()
    {
        MovementHorizontal = Input.GetAxis("Horizontal");
        MovementVertical = Input.GetAxis("Vertical");
    }

    public void Update()
    {
        if (Input.GetKeyDown(DashKeyCode))
        {
            DashPressed?.Invoke();
        }

        if (Input.GetKeyDown(DasDownhKeyCode))
        {
            DashDownPressed?.Invoke();
        }

        if (Input.GetKeyDown(JumpKeyCode))
        {
            JumpPressed?.Invoke();
        }  
    }
}