using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    public static PlayerHealth Instance { get; private set; }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            return;
        }
        
        Debug.LogError("There`s one more PlayerHealth in scene");
        Debug.Break();
    }
    
    public override void Death()
    {
    }
}
