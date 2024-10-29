using System;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    public float MovementHorizontal { get; private set; }
    public float MovementVertical { get; private set; }
    [field: SerializeField] public KeyCode DashKeyCode { get; private set; }
    [field: SerializeField] public KeyCode DasDownhKeyCode { get; private set; }
    [field: SerializeField] public KeyCode JumpKeyCode { get; private set; }
    [field: SerializeField] public KeyCode MainShootKeyCode { get; private set; }
    [field: SerializeField] public KeyCode ShotGunKeyCode { get; private set; }
    [field: SerializeField] public KeyCode RifleKeyCode { get; private set; }
    [field: SerializeField] public KeyCode RPGKeyCode { get; private set; }

    public bool MainShooting { get; private set; }

    
    public event Action DashPressedDown;
    public event Action DashDownwardsPressedDown;
    public event Action JumpPressedDown;

    public event Action MainShootPressedDown;
    public event Action MainShootPressedUp;
    
     public event Action ShotGunKeyPressedDown;
     public event Action RifleKeyPressedDown;
     public event Action RPGKeyPressedDown;

    public void GetMovingInputs()
    {
        MovementHorizontal = Input.GetAxis("Horizontal");
        MovementVertical = Input.GetAxis("Vertical");
    }

    public void Update()
    {
        if (Input.GetKeyDown(MainShootKeyCode))
        {
            MainShootPressedDown?.Invoke();
            MainShooting = true;
        }

        if (Input.GetKeyUp(MainShootKeyCode))
        {
            MainShootPressedUp?.Invoke();
            MainShooting = false;
        }

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

        if (Input.GetKeyDown(ShotGunKeyCode))
        {
            ShotGunKeyPressedDown?.Invoke();
        }
        
        if (Input.GetKeyDown(RifleKeyCode))
        {
            RifleKeyPressedDown?.Invoke();
        }
        
        if (Input.GetKeyDown(RPGKeyCode))
        {
            RPGKeyPressedDown?.Invoke();
        }
    }
}