using UnityEngine;
using System;

public class PlayerWeaponInputs : MonoBehaviour
{
    [field: SerializeField] public KeyCode MainShootKeyCode { get; private set; }
    [field: SerializeField] public KeyCode WeaponAbilityKeyCode { get; private set; }

    public bool MainShooting { get; private set; }

    public event Action MainShootPressedDown;
    public event Action MainShootPressedUp;

    public event Action WeaponAbilityPressedDown;


    private void Update()
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

        if (Input.GetKeyDown(WeaponAbilityKeyCode))
        {
            WeaponAbilityPressedDown?.Invoke();
        }
    }
}
