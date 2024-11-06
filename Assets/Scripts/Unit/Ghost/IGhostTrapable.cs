using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGhostTrapable 
{
    public IWeaponVisitor ObjectVisitor { get; }

    public void Trap(Ghost ghost);
    public void UnTrap();
}
