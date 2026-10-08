using System;
using UnityEngine;

public class PlayerSwitchWeaponInputs : MonoBehaviour
{
    public KeyCode ShotGunKeyCode => KeyBindings.Get(GameAction.Weapon1);
    public KeyCode RifleKeyCode => KeyBindings.Get(GameAction.Weapon2);
    public KeyCode RPGKeyCode => KeyBindings.Get(GameAction.Weapon2);
    public KeyCode RailgunKeyCode => KeyBindings.Get(GameAction.Weapon3);
    
    public event Action ShotGunKeyPressedDown;
    public event Action RifleKeyPressedDown;
    public event Action RPGKeyPressedDown;
    public event Action RailgunKeyPressedDown;

    private Settings _settings;

    private void Start()
    {
        _settings = Settings.Instance;
    }

    private void Update()
    {
        if (_settings.Open)
            return;

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