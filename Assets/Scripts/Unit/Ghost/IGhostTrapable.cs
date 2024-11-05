using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGhostTrapable
{
    [field: SerializeField] public IWeaponVisitor ObjectVisitor { get; set; }

    public void Trap();
    public void UnTrap();
}
