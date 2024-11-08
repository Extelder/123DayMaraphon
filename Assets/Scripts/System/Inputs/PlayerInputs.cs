using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    [field: SerializeField] public PlayerMovementInputs PlayerMovementInputs { get; private set; }
    [field: SerializeField] public PlayerWeaponInputs PlayerWeaponInputs { get; private set; }
    [field: SerializeField] public PlayerSwitchWeaponInputs PlayerSwitchWeaponInputs { get; private set; }
}