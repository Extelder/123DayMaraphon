using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LerpInPoint : MonoBehaviour
{
   [field: SerializeField] public Transform Target;
   [field: SerializeField] public Transform Point;
   
   public abstract void Lerp();
}
