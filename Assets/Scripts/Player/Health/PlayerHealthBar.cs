using Zenject;
using System;

public class PlayerHealthBar : HealthBar
{
    [Inject] private PlayerHealth _playerHealth;

    private void Awake()
    {
        OverrideHealth(_playerHealth);
    }
}
