using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UnlockShotGunOnTrigger : PlayerTrigger
{
    [SerializeField] private ParticleSystem _particle;

    [Inject] private PlayerCharacter _character;

    public override void OnTriggered()
    {
        _particle.transform.parent = null;
        _particle.transform.SetParent(null);
        _particle.Play();
        _character.WeaponSwitch.UnlockShotGun();
    }
}