using UnityEngine;

public abstract class State : MonoBehaviour
{
   public bool CanChanged = true;

   public abstract void Enter();

   public virtual void Exit() {}
}
