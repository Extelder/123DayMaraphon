using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelCheckPoint : MonoBehaviour
{
    public Vector3 LastCheckPoint { get; private set; }

    public void SetNewCheckPoint(Vector3 point)
    {
        LastCheckPoint = point;
    }
}