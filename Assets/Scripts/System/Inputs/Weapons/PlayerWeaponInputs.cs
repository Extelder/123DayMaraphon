using UnityEngine;
using System;

public class PlayerWeaponInputs : MonoBehaviour
{
    [field: SerializeField] public KeyCode MainShootKeyCode { get; private set; }
    [field: SerializeField] public KeyCode WeaponAbilityKeyCode { get; private set; }
    [field: SerializeField] public KeyCode ShotGunKeyCode { get; private set; }
    [field: SerializeField] public KeyCode RifleKeyCode { get; private set; }
    [field: SerializeField] public KeyCode RPGKeyCode { get; private set; }
    [field: SerializeField] public KeyCode RailgunKeyCode { get; private set; }

    public bool MainShooting { get; private set; }

    public event Action MainShootPressedDown;
    public event Action MainShootPressedUp;

    public event Action WeaponAbilityPressedDown;

    public event Action ShotGunKeyPressedDown;
    public event Action RifleKeyPressedDown;
    public event Action RPGKeyPressedDown;
    public event Action RailgunKeyPressedDown;

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

        if (Input.GetKeyDown(RailgunKeyCode))
        {
            RailgunKeyPressedDown?.Invoke();
        }
    }
}
