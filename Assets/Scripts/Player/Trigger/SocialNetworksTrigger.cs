using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialNetworksTrigger : PlayerTrigger
{
    [SerializeField] private string _socialNetwork;
    public override void OnTriggered()
    {
        Application.OpenURL(_socialNetwork);
    }
}
