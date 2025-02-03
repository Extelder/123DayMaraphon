using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WeaponUnlocker : MonoBehaviour
{
    [Inject] private PlayerCharacter _character;

    public void UnlockShotGun()
    {
        _character.WeaponSwitch.UnlockShotGun();
    }

    public void UnlockRifle()
    {
        _character.WeaponSwitch.UnlockRifle();
    }

    public void UnlockRPG()
    {
        _character.WeaponSwitch.UnlockRPG();
    }

    public void UnlockRailgun()
    {
        _character.WeaponSwitch.UnlockRailgun();
    }
}