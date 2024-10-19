using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/ New Weapon")]
public class WeaponItem : ScriptableObject
{
    public string Name;
    public int Id;
    public int DamagePerHit;
}