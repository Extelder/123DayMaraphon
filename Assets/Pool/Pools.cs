using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pools : MonoBehaviour
{
    [field: SerializeField] public Pool DefaultImpactPool { get; private set; }
    [field: SerializeField] public Pool DefaultProjectilePool { get; private set; }
    [field: SerializeField] public Pool GhostPool { get; private set; }
    [field: SerializeField] public Pool BloodExplodePool { get; private set; }
    [field: SerializeField] public Pool DashDownPool { get; private set; }
    [field: SerializeField] public Pool DashPool { get; private set; }
    [field: SerializeField] public Pool TrailPool { get; private set; }
}