using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OpenCloseAnimator : MonoBehaviour
{
    [field: SerializeField] public string OpenAnimatorBoolName { get; private set; } = "IsOpened";

    [field: SerializeField] public Animator Animator { get; private set; }

    public virtual void OpenAnimation()
    {
        Animator.SetBool(OpenAnimatorBoolName, true);
    }

    public virtual void CloseAnimation()
    {
        Animator.SetBool(OpenAnimatorBoolName, false);
    }
}