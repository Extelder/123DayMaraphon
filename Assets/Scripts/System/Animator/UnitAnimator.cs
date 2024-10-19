using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitAnimator : MonoBehaviour
{
    [field: SerializeField] public Animator Animator { get; private set; }

    public abstract void DisableAllBools();

    public void Idle()
    {
        DisableAllBools();
    }

    public void SetTrueAnimationBoolWithDisableOthers(string name)
    {
        DisableAllBools();
        SetAnimationBool(name, true);
    }

    public void SetAnimationBool(string nam, bool value)
    {
        Animator.SetBool(nam, value);
    }
}