using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerCharacter> (out PlayerCharacter playerCharacter))
        {
            Debug.Log("азазаз");
        }
    }
}
