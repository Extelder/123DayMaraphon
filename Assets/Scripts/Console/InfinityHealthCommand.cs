using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ED.SC;

public class InfinityHealthCommand : MonoBehaviour
{
    
    [Command]
    public void EnableImmortality()
    {
        PlayerCharacter.Instance.PlayerHitBox.Active = false;
    }
    
    [Command]
    public void DisableImmortality()
    {
        PlayerCharacter.Instance.PlayerHitBox.Active = true;
    }
}
