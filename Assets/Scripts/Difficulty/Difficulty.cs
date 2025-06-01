using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Difficulty/New")]
public class Difficulty : ScriptableObject
{
    public DifficultyType Type;
    public float DamageMultiplier;
    public float HealthDamageMultiplier;
}