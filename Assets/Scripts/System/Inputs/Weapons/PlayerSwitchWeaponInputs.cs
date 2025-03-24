using System;
using UnityEngine;

public class PlayerSwitchWeaponInputs : MonoBehaviour
{
    [field: SerializeField] public KeyCode ShotGunKeyCode { get; private set; }
    [field: SerializeField] public KeyCode RifleKeyCode { get; private set; }
    [field: SerializeField] public KeyCode RPGKeyCode { get; private set; }
    [field: SerializeField] public KeyCode RailgunKeyCode { get; private set; }
    
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