using UnityEngine;
using System;

public class PlayerWeaponInputs : MonoBehaviour
{
    [field: SerializeField] public KeyCode MainShootKeyCode { get; private set; }
    [field: SerializeField] public KeyCode KunitanaShootKeyCode { get; private set; }
    [field: SerializeField] public KeyCode WeaponAbilityKeyCode { get; private set; }
    [field: SerializeField] public KeyCode KunitanaUltimateShootKeyCode { get; private set; }

    public bool MainShooting { get; private set; }
    public bool KunitanaAttacking { get; private set; }
    public event Action MainShootPressedDown;
    public event Action MainShootPressedUp;

    public event Action KunitanaShootPressedDown;
    public event Action KunitanaShootPressedUp;

    public event Action WeaponAbilityPressedDown;
    public event Action WeaponAbilityPressedUp;
    public event Action KunitanaUltimateKeyDowm;
    public event Action KunitanaUltimateKeyUp;
    private Settings _settings;

    private void Start()
    {
        _settings = Settings.Instance;
    }

    private void Update()
    {
        if (_settings.Open)
            return;
        if (Input.GetKeyDown(KunitanaShootKeyCode))
        {
            KunitanaShootPressedDown?.Invoke();
            KunitanaAttacking = true;
        }

        if (Input.GetKeyUp(KunitanaShootKeyCode))
        {
            KunitanaShootPressedUp?.Invoke();
            KunitanaAttacking = false;
        }

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

        if (Input.GetKeyUp(WeaponAbilityKeyCode))
        {
            WeaponAbilityPressedUp?.Invoke();
        }

        if (Input.GetKeyUp(KunitanaUltimateShootKeyCode))
        {
            KunitanaUltimateKeyUp?.Invoke();
        }

        if (Input.GetKeyDown(KunitanaUltimateShootKeyCode))
        {
            KunitanaUltimateKeyDowm?.Invoke();
        }
    }
}