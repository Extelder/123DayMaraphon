using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public interface ISlashProjectile
{
    public float Damage { get; set; }
    public float Speed { get; set; }
    public float CharacterUpForce { get; set; }
    public Collider Collider { get; set; }
    
    public event Action Triggered;
}
